using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace SimpleSpinLock
{
    /// <summary>
    /// 简单的自旋锁 struct 更快释放，对内存友好
    /// </summary>
    public struct SimpleSpinLock
    {
        private Int32 m_ResourceInUse; //0 =false (默认)   ，1=true

        /// <summary>
        /// 上锁
        /// </summary>
        public void Enter()
        {
            while (true)
            {
                if (Interlocked.Exchange(ref m_ResourceInUse, 1) == 0)
                    return;
                //Thread.Sleep(0);//0 告诉Windows 主动放弃了Cpu为此线程分配的剩余时间片，强迫OS调用下一个线程 ，单下一个线程也有可能是这个线程
                //Thread.Sleep(-1);// 强迫Cup 切换下一个线程，换句话说总会强迫切换上下文
                //Thread.Sleep(-1);// 告诉Windows 用于不要在调度此线程
                var boolRet = Thread.Yield();//告诉Windows 如果有已经准备好的线程就执行其他线程，并主动放弃剩余CUP时间片 返回Ture。当这个线程执行好后，调用Yield线程再次被调度，并获得全新的时间片运行
                //如果没有发现已准备好的线程，Yield会返回False 调用Yield的线程继续运行它的时间片
            }
        }
        /// <summary>
        /// 解锁
        /// </summary>
        public void Leave()
        {
            Volatile.Write(ref m_ResourceInUse, 0);// 等价于 如果需要原来值时调用Interlocked.Exchange(ref m_ResourceInUse, 0)
        }

    }
}
