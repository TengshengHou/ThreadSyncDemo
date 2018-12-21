using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterlockStructure
{
    /// <summary>
    /// 异步协调器状态
    /// </summary>
    public enum CoordinationStatus { AllDone, Timeout, Cancel };
    /// <summary>
    /// 异步协调器
    /// </summary>
    public sealed class AsyncCoordinator
    {
        private Int32 m_OpCount = 1;//Allbegun内部调用JustEnded 来递减它
        private Int32 m_statusReported = 0;//0=false 1=true 是否已做状态处理
        private Action<CoordinationStatus> m_callback;
        private Timer m_time;

        //该方法必须在发起一次操作之前调用
        public void AboutToBegin(Int32 opsToAdd = 1)
        {
            Interlocked.Add(ref m_OpCount, opsToAdd);
        }
        public void JustEnded()
        {
            if (Interlocked.Decrement(ref m_OpCount) == 0)
                ReportStatus(CoordinationStatus.AllDone);
        }

        public void AllBegun(Action<CoordinationStatus> callback, Int32 timeout = Timeout.Infinite)
        {
            m_callback = callback;
            if (timeout != Timeout.Infinite)
                m_time = new Timer(TimeExpired, null, timeout, Timeout.Infinite);
            JustEnded();
        }
        private void TimeExpired(object o)
        {
            ReportStatus(CoordinationStatus.Timeout);
        }
        public void Cancel()
        {
            ReportStatus(CoordinationStatus.Cancel);
        }
        private void ReportStatus(CoordinationStatus status)
        {

            Console.WriteLine("我被调用了很多次，但是只有一个线程我能放行 当前线程ID {0}", Thread.CurrentThread.ManagedThreadId);
            if (Interlocked.Exchange(ref m_statusReported, 1) == 0)
                m_callback(status);

        }
    }
}
