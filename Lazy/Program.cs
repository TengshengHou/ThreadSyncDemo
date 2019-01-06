using System;
using System.Threading;

namespace Lazy
{
    class Program
    {
        //static Lazy<DateTime> l = new Lazy<DateTime>(DateTime.Now);
        static void Main(string[] args)
        {
            Lazy<DateTime> l = new Lazy<DateTime>(DateTime.Now);

            Console.WriteLine(l.Value.ToString());
            Thread.Sleep(1000 * 10);
            Console.WriteLine(l.Value.ToString());


            Console.ReadLine();
        }


    }

}
