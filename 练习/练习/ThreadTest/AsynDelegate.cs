using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 练习.ThreadTest
{
    /// <summary>
    /// 异步委托
    /// </summary>
    class AsynDelegate
    {
        public static void AsynDelegateMain()
        {
            TakesAWhileDelegate d = TakesAWhile;

            IAsyncResult ar = d.BeginInvoke(1, 2000, null, null);

            //isCompleted异步线程是否运行结束
            /*while (!ar.IsCompleted)
            {
                Console.Write(".");
                Thread.Sleep(50);
            }*/

            //AsyncWaitHandle等待句柄
            while (true)
            {
                Console.Write(".");
                //设置等待最长时间
                if (ar.AsyncWaitHandle.WaitOne(50, false))
                {
                    Console.WriteLine("Can get the result now");
                    break;
                }
            }

            int result = d.EndInvoke(ar);
            Console.WriteLine($"result:{result}");
        }

        public delegate int TakesAWhileDelegate(int data, int ms);

        public static int TakesAWhile(int data, int ms)
        {
            Console.WriteLine("TakesAWhile start");
            Thread.Sleep(ms);
            Console.WriteLine("\nTakesAWhile over");
            return ++data;
        }
    }

   
}
