using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSynchronization
{
    public class Program
    {
        internal static int count;
        static AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        static ManualResetEvent manualResetEvent = new ManualResetEvent(true);
        static void Main(string[] args)
        {
            SpinWait spinWait = new SpinWait();
            spinWait.SpinOnce();
        }

        public static void Test1()
        {
            autoResetEvent.Set();
            Thread.Sleep(2000);
        }
    }



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
}

