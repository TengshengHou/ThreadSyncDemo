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
        //System.Threading.SpinLock spinLock = new System.Threading.SpinLock(); //FCL默认定义的自旋锁 内部用的Thread.SpinWait实现


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
