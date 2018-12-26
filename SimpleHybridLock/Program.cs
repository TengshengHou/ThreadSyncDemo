using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHybridLockTest
{
    public class Program
    {
        private static Int32 count;
        private static SimpleHybridLock simpleHybridLock = new SimpleHybridLock();
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 200; i++)
            {
                var t = Task.Run(() =>
                {
                    simpleHybridLock.Enter();
                    ++count;
                    Console.WriteLine(count);
                    simpleHybridLock.Leave();
                });
                tasks.Add(t);
            }


            Task.WaitAll(tasks.ToArray());
            Console.ReadLine();
        }
    }
}
