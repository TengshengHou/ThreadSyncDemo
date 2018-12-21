using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterlockStructure
{
    class Program
    {
        static void Main(string[] args)
        {
            MultiWebRequests multiWebRequests = new MultiWebRequests(10000);
            Console.ReadLine();
        }
    }
}
