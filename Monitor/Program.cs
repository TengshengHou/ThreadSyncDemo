using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorText
{
    class Program
    {
        static object m_lock = new object();
        static bool m_coundition = false;
        static void Main(string[] args)
        {

            Task.Run(() =>
            {
                Thread1();
            });
            Thread.Sleep(5);
            Task.Run(() =>
            {
                Thread2();
            });





            Console.ReadLine();

        }
        static void Thread1()
        {


            Console.WriteLine("Thread1进入");
            Monitor.Enter(m_lock);//获取一个互斥锁  获取锁，如果未成功获得（其他线程在锁资源）挂起线程等待信号
            Console.WriteLine("Thread1");
            //在锁中 原子性 地测试符合条件
            while (!m_coundition)
            {
                Console.WriteLine("Thread1进入自旋并临时释放锁");
                //条件不满足，就等待另外一个线程更改条件
                Monitor.Wait(m_lock);//临时释放锁，其他线程能获取它
            }
            //满足条件，处理数据
            Console.WriteLine("业务操作");

            Console.WriteLine("Thread1永久释放");
            Monitor.Exit(m_lock);//永久释放锁
        }

        static void Thread2()
        {
            Console.WriteLine("Thread2进入");
            Monitor.Enter(m_lock);//获取一个互斥锁
            Console.WriteLine("Thread2");
            //处理数据并修改条件
            m_coundition = true;
            Thread.Sleep(1000 * 10);
            Monitor.Pulse(m_lock);//锁释放之后唤醒一个正在等待的线程
            Console.WriteLine("Thread2永久释放锁");
            Monitor.Exit(m_lock);//永久释放锁
        }
    }


}
