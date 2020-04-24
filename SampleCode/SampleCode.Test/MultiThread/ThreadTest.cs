using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.MultiThread
{
    [TestFixture]
    public class ThreadTest
    {
        /// <summary>
        /// 创建并启动一个线程
        /// </summary>
        [Test]
        public void NewThreadTest()
        {
            //方法1 给入方法
            var t = new Thread(RunMain);
            t.Start();

            //方法2 Lambda表达式
            var t1 = new Thread(() => Console.WriteLine("t1Run:" + Thread.CurrentThread.ManagedThreadId));
            t1.Start();
        }

        public void RunMain()
        {
            Console.WriteLine("ThreadMainRun:" + Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// 向线程传递数据
        /// </summary>
        [Test]
        public void PassOnDataToThreadTest()
        {
            //方法1 使用委托ParameterizedTreadStart
            var t2 = new Thread(ParameterizedTreadStart);
            t2.Start(new Person { Age = 1, FirstName = "AAA" });
            Console.WriteLine("main:" + Thread.CurrentThread.ManagedThreadId);

            //方法2 给新线程传递数据的另一种方式是定义一个类,在其中定义需要的字段,将
            //线程的主方法定义为类的一个实例方法
            var obj = new ThreadClass("BBB");
            var t3 = new Thread(obj.ThreadMain);
            t3.Start();
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

        /// <summary>
        /// 后台线程
        /// 进程中只要有一个前台线程在运行，那么进程就处于激活状态
        /// 后台线程在进程终止时便终止了
        /// </summary>
        [Test]
        public void BackgroundThreadTest()
        {
            var t = new Thread(RunMain2)
            {
                IsBackground = true,
                Name = "BackgroundThreadTest",
            };
            t.Start();
            Console.WriteLine("Main thread ending now");
        }

        public void RunMain2()
        {
            Console.WriteLine("ThreadMain3Start");
            Thread.Sleep(3000);
            Console.WriteLine("ThreadMain3End");
        }

        /// <summary>
        /// 线程优先级和线程生命周期
        /// </summary>
        [Test]
        public void OtherThreadInfoTest()
        {
            //线程优先级
            //操作系统根据线程优先级通过线程调度器调度优先级最高的线程在cpu上运行
            //当线程优先级相同时，会采用时分复用对线程进行调用
            //在.net中线程优先级可以通过Tread的Priority进行设置，枚举为ThreadPriority
            var t = new Thread(RunMain2)
            {
                Priority = ThreadPriority.Normal
            };

            //线程生命周期
            //见书操作系统
        }
    }

    public class Person
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 国籍
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 成功次数
        /// </summary>
        public int SucceedNum { get; set; }

        /// <summary>
        /// 获取Person集合
        /// </summary>
        /// <returns></returns>
        public static List<Person> GetPeopleList()
        {
            List<Person> list = new List<Person>
            {
                new Person()
                {
                    Id = 1,
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Country = "China",
                    Age = 20,
                    SucceedNum = 3
                },
                new Person()
                {
                    Id = 2,
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Country = "China",
                    Age = 8,
                    SucceedNum = 6
                },
                new Person()
                {
                    Id = 3,
                    FirstName = "FirstName3",
                    LastName = "LastName3",
                    Country = "Japan",
                    Age = 31,
                    SucceedNum =8
                },
                new Person()
                {
                    Id = 4,
                    FirstName = "FirstName4",
                    LastName = "LastName4",
                    Country = "Japan",
                    Age = 40,
                    SucceedNum =17
                },
            };

            return list;
        }
    }

    public class ThreadClass
    {
        public string Message { get; set; }

        public ThreadClass() { }

        public ThreadClass(string message)
        {
            Message = message;
        }

        public void ThreadMain()
        {
            Console.WriteLine("Message:" + Message);
        }
    }
}