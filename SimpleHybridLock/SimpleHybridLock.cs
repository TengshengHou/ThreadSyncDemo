using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHybridLockTest
{
    public sealed class SimpleHybridLock : IDisposable
    {
        /// <summary>
        /// 用于基元用户模式构造 （interlocked的方法）使用
        /// </summary>
        private Int32 m_waiters = 0;
        /// <summary>
        /// 基元内核模式
        /// </summary>
        private AutoResetEvent m_waiterLock = new AutoResetEvent(false);

        public void Enter()
        {
            //支出这个线程小要获得锁
            if (Interlocked.Increment(ref m_waiters) == 1)
                return;//锁可以自由使用，无需竞争，直接返回，（说明没有线程在竞争资源，所有不用上锁）
            //领一个线程由于锁（发生竞争），使这个线程等待
            m_waiterLock.WaitOne();//这个产生很大性能影响（FCL->windows32）
            //waitOne返回后 这个线程拿到了锁

        }
        public void Leave()
        {
            //这个线程准备释放锁
            if (Interlocked.Decrement(ref m_waiters) == 0)
                return;//没有其他线程正在等待 ，直接返回

            //如果能执行到这行代码说明，有其他线程正在堵塞 唤醒其中一个
            m_waiterLock.Set();//这个产生很大性能影响（FCL->windows32）
        }


        /// <summary>
        /// 不要手动调用，交给GC回收
        /// </summary>
        public void Dispose() { m_waiterLock.Dispose(); }
    }
}
