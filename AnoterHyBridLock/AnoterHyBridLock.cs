using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnoterHyBridLockTest
{
    public class AnoterHyBridLock : IDisposable
    {
        //inr32 由基元用户模式构造（interlocked的方法）使用
        private Int32 m_waiters = 0;
        //AutoResetEvent 是基元内核模式构造
        private AutoResetEvent m_waiterLock = new AutoResetEvent(false);
        //这个字段控制自旋，希望能得到性能提升（自旋期间线程m_waiters被逻辑释放，省去了进入内核模式构造过程）
        private Int32 m_spincount = 4000;
        private Int32 m_owningThreadId = 0, m_recursion = 0;
        public void Enter()
        {
            Int32 threadId = Thread.CurrentThread.ManagedThreadId;
            //如果调用线程已经有锁，递增递归计数并返回
            if (threadId == m_owningThreadId)
            {
                m_recursion++;
                return;
            }
            //调用线程不拥有锁，自旋一小会尝试获取他。
            SpinWait SpinWait = new SpinWait();
            for (int i = 0; i < m_spincount; i++)
            {
                //Interlocked.CompareExchange判断两个值是否相等，如果相等赋值前者为1
                if (Interlocked.CompareExchange(ref m_waiters, 1, 0) == 0)//当第一个线程进来时候循环第一次时跳入GotLock
                {
                    goto GotLock;
                }
                //黑科技：给其他线程运行的机会 ，希望锁会被释放
                SpinWait.SpinOnce();
            }
            if (Interlocked.Increment(ref m_waiters) > 1)
            {
                m_waiterLock.WaitOne();
            }
            GotLock:
            m_owningThreadId = threadId; m_recursion = 1;
        }
        /// <summary>
        /// 调用此Leave时不要用Task调用
        /// </summary>
        public void Leave()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (threadId != m_owningThreadId)
                throw new SynchronizationLockException("Lock not owned by Calling thread");
            //递减递归计数，如果这个线程仍然拥有锁，那么直接返回（逻辑锁）
            if (--m_recursion > 0) //此处不用内存栅栏，因为此时以保证只有一个线程在处理
                return;
            m_owningThreadId = 0;
            //如果没有其他线程在等待直接返回
            if (Interlocked.Decrement(ref m_waiters) == 0)
            {
                return;
            }
            //有其他线程在等待 ，唤醒其中一个
            m_waiterLock.Set();//性能损耗

        }

        public void Dispose()
        {
            m_waiterLock.Dispose();
        }

    }
}
