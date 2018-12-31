using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnoterHyBridLockTest
{
    class Program
    {
        private static Int32 count;
        static void Main(string[] args)
        {

            using (AnoterHyBridLock anoterHybridLock = new AnoterHyBridLock())
            {
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < 20000; i++)
                {
                    var t = Task.Run(() =>
                    {
                        anoterHybridLock.Enter();
                        //Thread.Sleep(10);
                        ++Program.count;
                        Console.WriteLine(Program.count);
                        if (i % 2 == 0)//测试 Leave在其他线程中调用异常情况
                        {
                            Task.Run(() =>
                            {
                                anoterHybridLock.Leave();
                            });
                        }
                        else
                            anoterHybridLock.Leave();
                    });
                    tasks.Add(t);
                }
                Task.WaitAll(tasks.ToArray());
            }
            Console.ReadLine();

        }
    }
}
