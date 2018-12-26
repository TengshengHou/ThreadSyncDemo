using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MutexText
{
    /// <summary>
    /// 互斥体的特点。 调用 ReleaseMutex的必须是获得Mutex的线程
    /// 一次只释放一个线程 
    /// 支持递归调用
    /// </summary>
    class Program
    {
        static int count;
        static Mutex mutex = new Mutex(false);
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 2000; i++)
            {
                var t = Task.Run(() =>
                {
                    mutex.WaitOne();

                    //Thread.Sleep(10);
                    ++Program.count;
                    Console.WriteLine(Program.count);
                    mutex.ReleaseMutex();
                });
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
            Console.ReadLine();
        }

    }
}
