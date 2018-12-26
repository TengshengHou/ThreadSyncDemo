using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Event
{
    class Program
    {
        internal static int count;
        static AutoResetEvent autoResetEvent = new AutoResetEvent(true);

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
                    Add1();

                    autoResetEvent.Set();

                });
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("ok");
            Console.ReadLine();
        }
        public static void Add1()
        {
            ++count;
            Console.WriteLine(count);
        }
    }




}