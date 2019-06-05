using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetTest.ThreadTest
{
    /// <summary>
    /// 异步委托
    /// </summary>
    class AsynDelegate
    {
        public static void AsynDelegateMain()
        {
            AsynDelegate test = new AsynDelegate();
            test.Method3();

        }

        /// <summary>
        /// 投票
        /// </summary>
        public void Method1()
        {
            TakesAWhileDelegate d = TakesAWhile;

            IAsyncResult ar = d.BeginInvoke(1, 2000, null, null);

            //isCompleted异步线程是否运行结束
            while (!ar.IsCompleted)
            {
                Console.Write(".");
                Thread.Sleep(50);
            }
            int result = d.EndInvoke(ar);
            Console.WriteLine($"result:{result}");
        }

        /// <summary>
        /// 等待句柄
        /// </summary>
        public void Method2()
        {
            TakesAWhileDelegate d = TakesAWhile;
            IAsyncResult ar = d.BeginInvoke(1, 2000, null, null);

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

        /// <summary>
        /// 异步回调
        /// </summary>
        public void Method3()
        {
            TakesAWhileDelegate d = TakesAWhile;
            //第三个参数可以用lambda表达式代替，并不需要传递第四个参数可以直接进行使用
            IAsyncResult ar = d.BeginInvoke(1, 2000, TakesAWhileComleted, d);
        }

        public void TakesAWhileComleted(IAsyncResult ar)
        {
            if (ar == null) return;
            //BeginInvoke的最后一个参数可以用AsyncState读取
            var d1 = ar.AsyncState as TakesAWhileDelegate;
            var result = d1.EndInvoke(ar);
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
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
