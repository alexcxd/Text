using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 练习.ThreadTest
{
    class ThreadClass
    {
        public static void ThreadClassMain()
        {
            ThreadClass test = new ThreadClass();
            test.NewThread();
        }

        /// <summary>
        /// 创建并启动一个线程
        /// </summary>
        public void NewThread()
        {
            //方法1 给入方法
            Thread t = new Thread(ThreadMain);
            t.Start();

            //方法2 Lambda表达式
            Thread t1 = new Thread(() => Console.WriteLine("Run:" + Thread.CurrentThread.ManagedThreadId));
            t1.Start();

            Console.WriteLine("main:" + Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// 给线程传递数据
        /// </summary>
        public void PassOnThread()
        {

        }

        public void ThreadMain()
        {
            Console.WriteLine("Run:" + Thread.CurrentThread.ManagedThreadId);
        }
    }
}
