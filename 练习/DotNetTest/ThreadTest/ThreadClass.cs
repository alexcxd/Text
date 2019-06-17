using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetTest.ThreadTest
{
    public class ThreadClass
    {
        public string Message { get; set; }

        public ThreadClass() { }

        public ThreadClass(string message)
        {
            Message = message;
        }


        public static void ThreadClassMain()
        {
            var test = new ThreadClass();
            //test.NewThread();
            //test.PassOnDataToThread();
            test.BackgroundThread();
        }

        /// <summary>
        /// 创建并启动一个线程
        /// </summary>
        public void NewThread()
        {
            //方法1 给入方法
            var t = new Thread(ThreadMain);
            t.Start();

            //方法2 Lambda表达式
            var t1 = new Thread(() => Console.WriteLine("t1Run:" + Thread.CurrentThread.ManagedThreadId));
            t1.Start();
        }

        /// <summary>
        /// 向线程传递数据
        /// </summary>
        public void PassOnDataToThread()
        {
            //方法1 使用委托ParameterizedTreadStart
            var t2 = new Thread(ParameterizedTreadStart);
            t2.Start(new Person { Age = 1, FirstName = "AAA" });
            Console.WriteLine("main:" + Thread.CurrentThread.ManagedThreadId);

            //方法2 给新线程传递数据的另一种方式是定义一个类,在其中定义需要的字段,将
            //线程的主方法定义为类的一个实例方法
            var obj = new ThreadClass("BBB");
            var t3 = new Thread(obj.ThreadMain2);
            t3.Start();
        }

        /// <summary>
        /// 后台线程
        /// 进程中只要有一个前台线程在运行，那么进程就处于激活状态
        /// 后台线程在进程终止时便终止了
        /// </summary>
        public void BackgroundThread()
        {
            var t = new Thread(ThreadMain3)
            {
                IsBackground = true,
                Name = "BackgroundThreadTest",
            };
            t.Start();
            Console.WriteLine("Main thread ending now");
        }

        /// <summary>
        /// 线程优先级和线程生命周期
        /// </summary>
        public void OtherThreadInfo()
        {
            //线程优先级
            //操作系统根据线程优先级通过线程调度器调度优先级最高的线程在cpu上运行
            //当线程优先级相同时，会采用时分复用对线程进行调用
            //在.net中线程优先级可以通过Tread的Priority进行设置，枚举为ThreadPriority
            var t = new Thread(ThreadMain3)
            {
                Priority = ThreadPriority.Normal
            };

            //线程生命周期
            //见书操作系统
        }

        /// <summary>
        /// 给线程传递数据
        /// 必须实现委托ParameterizedTreadStart委托
        /// </summary>
        public void ParameterizedTreadStart(object o)
        {
            var person = o as Person ?? new Person();
            Console.WriteLine($"FirstName:{person.FirstName}, Age:{person.Age}");
        }

        public void ThreadMain()
        {
            Console.WriteLine("ThreadMainRun:" + Thread.CurrentThread.ManagedThreadId);
        }

        public void ThreadMain2()
        {
            Console.WriteLine("Message:" + Message);
        }

        public void ThreadMain3()
        {
            Console.WriteLine("ThreadMain3Start");
            Thread.Sleep(3000);
            Console.WriteLine("ThreadMain3End");
        }
    }
}
