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
    public class AsynDelegate
    {
        public static void AsynDelegateMain()
        {
            var test = new AsynDelegate();
            //test.Method1();
            //test.Method2();
            test.Method3();

        }

        /// <summary>
        /// 投票
        /// </summary>
        public void Method1()
        {
            TakesAWhileDelegate d = TakesAWhile;

            var ar = d.BeginInvoke(1, 2000, null, null);

            //isCompleted异步线程是否运行结束
            while (!ar.IsCompleted)
            {
                Console.Write(".");
                Thread.Sleep(50);
            }
            var result = d.EndInvoke(ar);
            Console.WriteLine($"result:{result}");
        }

        /// <summary>
        /// 等待句柄
        /// </summary>
        public void Method2()
        {
            TakesAWhileDelegate d = TakesAWhile;
            var ar = d.BeginInvoke(1, 2000, null, null);

            var i = 0;
            while (true)
            {
                Console.Write(++i);
                //设置等待最长时间
                //使用AsyncWaitHandle返回一个WaitHandle句柄，他可以等待线程完成其任务
                //第一个参数为等待时间
                //第二个参数，如果等待之前先退出上下文的同步域（如果在同步上下文中），并在稍后重新获取它，则为 true；否则为 false。
                if (ar.AsyncWaitHandle.WaitOne(50, false))
                {
                    Console.WriteLine("Can get the result now");
                    break;
                }
            }
            
            var result = d.EndInvoke(ar);
            Console.WriteLine($"result:{result}");
        }

        /// <summary>
        /// 异步回调
        /// BeginInvoke代表开始调用，返回一个代表异步结果的对象（IAsyncResult）
        /// EndInvoke代表结束调用，返回调用函数结果
        /// </summary>
        public void Method3()
        {
            //BeginInvoke总是会带AsyncCallback和object参数
            //AsyncCallback会在线程结束时调用，object代表用户自定义对象，该对象将传递到回调方法中,也就是AsyncState
            //AsyncCallback可以用lambda表达式代替，并不需要传递第四个参数可以直接进行使用
            TakesAWhileDelegate d = TakesAWhile;
            var ar1 = d.BeginInvoke(1, 2000, TakesAWhileComleted, d);
            /*var ar2 = d.BeginInvoke(1, 2000, ar =>
            {
                var result = d.EndInvoke(ar);
                Console.WriteLine($"TreadId:{Thread.CurrentThread.ManagedThreadId}");
            }, null);*/

        }

        /// <summary>
        /// 需要满足委托AsyncCallback的要求
        /// </summary>
        /// <param name="ar"></param>
        public void TakesAWhileComleted(IAsyncResult ar)
        {
            if (ar == null) return;
            //BeginInvoke的最后一个参数可以用AsyncState读取
            var d1 = ar.AsyncState as TakesAWhileDelegate;
            //EndInvoke方法检测异步调用的结果。如果异步调用尚未完成，EndInvoke将阻塞调用线程，直到它完成。
            var result = d1.EndInvoke(ar);
            Console.WriteLine($"TreadId:{Thread.CurrentThread.ManagedThreadId}");
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
