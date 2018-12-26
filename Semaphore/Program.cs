using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreTest
{
    /// <summary>
    /// 内核维护一个Int32变量，信号了为0时 在信号了上等待的线程会堵塞，信号了大于0是解除堵塞
    /// 
    /// AutoResetEvent在性黑商和最大技术为1的信号量非常相似( semaphore.Release(1) )
    /// </summary>
    class Program
    {
        internal static int count;
        static Semaphore semaphore = new Semaphore(1, 10000);//初始化设置内部户Int32变量为1  第一次调用waitOne不会堵塞
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 200; i++)
            {
                var t = Task.Run(() =>
                {
                    //Console.WriteLine("来了老弟" + Program.count);
                    semaphore.WaitOne();
                    //Thread.Sleep(10);
                    ++Program.count;
                    Console.WriteLine(Program.count);
                    semaphore.Release(1);//唤醒等待线程中的某N个线程，此处为唤醒1个

                });
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("ok");
            Console.ReadLine();
        }



    }
}
