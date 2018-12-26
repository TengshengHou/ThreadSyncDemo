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
            List<Task> tasks = new List<Task>();
            autoResetEvent.Set();
            for (int i = 0; i < 200; i++)
            {
                var t = Task.Run(() =>
                  {
                      //Console.WriteLine("来了老弟" + Program.count);

                      autoResetEvent.WaitOne();
                     
                      //Thread.Sleep(10);
                      ++Program.count;
                      Console.WriteLine(Program.count);
                      autoResetEvent.Set();
                  
                  });
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());

            //Task.Run(() =>
            //{
            //    Thread.Sleep(2000);
            //    autoResetEvent.Set();
            //});
            //Console.WriteLine(autoResetEvent.Reset());
            //autoResetEvent.WaitOne();

            //new Example().Begin();


            Console.WriteLine("ok");
            Console.ReadLine();
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

