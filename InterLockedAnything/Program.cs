using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterLockedAnything
{
    class Program
    {
        static int count;
        static void Main(string[] args)
        {
            //Morpher<int, string> morpher = A;

            for (int i = 0; i < 200; i++)
            {
                Task.Run(() =>
                {
                    Morph<Int32, string>(ref count, null, A);
                });
            }
            Console.ReadLine();
        }

        public static int A(int a, string b, out int c)
        {
            c = 1;
            count++;
            Console.WriteLine("ThreadID={0}  ---- {1}", Thread.CurrentThread.ManagedThreadId, count);
            return a;
        }


        public static Int32 Maximum(ref Int32 target, Int32 value)
        {
            Int32 currentVal = target;
            Int32 startVal;
            Int32 desiredVal;
            do
            {
                //记录这一次循环迭代的起始值
                startVal = currentVal;

                //你的业务逻辑 此Demo为 基于StartVal 和 val计算 desiredVal （其实就是求Target 和Value 谁最大，期间有肯能有其他线程赋值操作Target）
                desiredVal = Math.Max(startVal, value);
                //错误示例：注意CI线程在这里可能被“抢占” 所以一下代码不是原子性的：
                //if(target==stratVal) target = desiredVal

                //正确示例： 原子性操作 CompareExchange方法 ，他返回在target在（可能）备方法修改之前的值
                currentVal = Interlocked.CompareExchange(ref target, desiredVal, startVal);


                //如果target的值 在这一次循环迭代中被其他线程改变，就重复
            } while (startVal != currentVal);
            return desiredVal;
        }

        delegate Int32 Morpher<TResult, TArgument>(Int32 startValue, TArgument argument, out TResult morphResult);
        static TResult Morph<TResult, TArgument>(ref Int32 target, TArgument argument, Morpher<TResult, TArgument> morpher)
        {
            TResult morphResult;

            Int32 currentVal = target;
            Int32 startVal;
            Int32 desiredVal;

            do
            {
                startVal = currentVal;
                desiredVal = morpher(target, argument, out morphResult);
                currentVal = Interlocked.CompareExchange(ref target, desiredVal, startVal);
            } while (startVal != currentVal);
            return morphResult;
        }
    }
}
