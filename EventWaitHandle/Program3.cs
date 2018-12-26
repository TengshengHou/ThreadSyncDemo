using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Event
{
    /// <summary>
    /// ManualResetEvent AutoResetEvent区别
    /// 
    /// 
    /// ManualResetEvent： 
    /// 理解WaitOne 只是等信号号，如果EventWaitHandle信号为Ture WaitOne 线程唤醒并运行
    /// Set 设置信号为Ture 所有WaitOne线程被唤醒
    /// Reset 设置信号为False //所有WaitOne线程等待
    /// 
    /// AutoResetEvent：AutoResetEvent以ManualResetEvent 但是AutoResetEvent特性是WaitOne唤醒后 内核将事件自动重置为False 其他线程堵塞
    /// 
    /// AutoResetEvent适合多线程中排队去做一件事 适合操作共享资源
    /// ManualResetEvent ，一起做一个事情一起停止。不适合操作共享资源
    /// </summary>
    public class Example
    {
        /// <summary>
        /// 预备信号，准备发送，初始化
        /// </summary>
        public EventWaitHandle flag;
        public void Begin()
        {
            // flag = new ManualResetEvent(false);
            flag = new AutoResetEvent(false);
            Thread th1 = new Thread(() =>
            {
                flag.WaitOne();

                Thread.Sleep(1000);
                Console.WriteLine("第一个线程已经通过……");
                //flag.Set();

            });
            Thread th2 = new Thread(() =>
            {
                flag.WaitOne();
                Thread.Sleep(1000);
                Console.WriteLine("第二个线程已经通过……");
                //flag.Set();

            });
            Thread th3 = new Thread(() =>
            {
                flag.WaitOne();
                Thread.Sleep(1000);
                Console.WriteLine("第三个线程已经通过……");
                //flag.Set();

            });

            th1.IsBackground = true;
            th1.Start();
            th2.IsBackground = true;
            th2.Start();
            th3.IsBackground = true;
            th3.Start();
            flag.Set();
            Thread.Sleep(1000);

            Console.WriteLine("A");
            Console.WriteLine("B");
        }
    }
    class Program2
    {

        static void Main(string[] args)
        {
            new Example().Begin();
            Console.WriteLine("ok");
            Console.ReadLine();
        }
    }

}