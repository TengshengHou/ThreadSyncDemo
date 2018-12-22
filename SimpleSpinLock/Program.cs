using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSpinLock
{
    class Program
    {
        internal static int count;
        private static SimpleSpinLock SimpleSpinLock = new SimpleSpinLock();
        static void Main(string[] args)
        {
            for (int i = 0; i < 200; i++)
            {
                Task.Run(() =>
                {
                    SimpleSpinLock.Enter();
                    //Thread.Sleep(10);
                    ++Program.count;
                    Console.WriteLine(Program.count);
                    SimpleSpinLock.Leave();
                });
            }
            Console.ReadLine();
        }
    }
}
