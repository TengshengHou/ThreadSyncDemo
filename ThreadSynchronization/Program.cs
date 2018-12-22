using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadSynchronization
{
    public class Program
    {
        public static Int32 count;
        static void Main(string[] args)
        {
            bool lockBool = false;
            System.Threading.SpinLock spinLock = new System.Threading.SpinLock();
            spinLock.Enter(ref lockBool);
            spinLock.Exit();
        }
    }
}
