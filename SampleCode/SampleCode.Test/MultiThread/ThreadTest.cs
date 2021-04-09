using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.MultiThread
{
    /// <summary>
    /// 线程
    /// </summary>
    [TestFixture]
    public class ThreadTest
    {
        #region 创建并启动一个线程

        /// <summary>
        /// 创建并启动一个线程
        /// </summary>
        [Test]
        public void NewThreadTest()
        {
            //方法1 构造器接受一个ThreadStart的委托
            var t = new Thread(RunMain);
            //每一个线程都会有一个Name属性用作调试用途, 在Visual Studio中线程的名字会显示在Thread窗口
            t.Name = "TestThread";
            //线程一旦启动，线程的属性IsAlive就会置为true,
            //当Thread的构造函数接收的委托执行完成后, 线程就会停止, 停止的线程无法再次启动
            t.Start();
            //可以调用Thread.Join方法等待线程结束
            //Join可以指定一个超时时间, 若在指定的超时时间内结束, 则返回true
            t.Join();

            //方法2 Lambda表达式
            var t1 = new Thread(() => Console.WriteLine("t1Run:" + Thread.CurrentThread.ManagedThreadId));
            t1.Start();
        }

        public void RunMain()
        {
            //Thread.CurrentThread会返回当前正在执行的线程
            Console.WriteLine("ThreadMainRun:" + Thread.CurrentThread.ManagedThreadId);
        }

        #endregion

        #region 线程的阻塞

        /// <summary>
        /// 线程的阻塞
        /// </summary>
        [Test]
        public void BlockedThreadTest()
        {
            //当线程由于特定原因暂停执行, 那么它就是阻塞的
            //阻塞的线程会立刻交出它的处理器时间片, 并在之后不再消耗处理器时间
            //阻塞并非0开销, 每一个线程在存货时会占用1MB的内存, 并对CLR和操作系统带来持续性的管理开销
            //可以使用ThreadState属性测试线程的阻塞状态
            var blocked = (Thread.CurrentThread.ThreadState & ThreadState.WaitSleepJoin) != 0;
            //当线程被阻塞或者解除阻塞时, 操作系统会进行一次上下文切换, 会造成1到2毫秒左右的开销

            //I/O密集:一个操作的绝大部分时间都在等待时间的发生
            //计算密集:一个操作的大部分时间都用于执行大量的CPU操作

            //自旋和阻塞
            //同步的I/O密集操作的大部分时间都花费在阻塞线程上, 但也可能在一个定期循环中自旋
            //实际上自旋是将I/O密集转化为计算密集(自旋过程中, CLR和操作系统会认为这个线程正在执行计算, 因此会分配资源)
            //非常短暂的自旋是高效的, 因为它避免了上下文的切换带来的延迟和开销
            var time = DateTime.Now.AddSeconds(3);
            while (DateTime.Now < time)
            {
                Thread.Sleep(10);
            }
        }

        #endregion

        #region 本地状态和共享状态

        private static bool _done;

        /// <summary>
        /// 本地状态和共享状态
        /// </summary>
        [Test]
        public void ThreadLocalVariableStateTest()
        {
            //CLR为每一个线程分配独立的内存栈, 从而保证局部变量的隔离
            //下面的示例中会输出10个?, 因为两个线程之间不会共享局部变量i
            new Thread(Go1).Start();
            Go1();
            //如果不同的线程拥有同一对象的引用, 则这些线程之间共享数据
            //下面示例仅会输出1个Done, 因为两个线程共享变量_done
            //但是当前示例是线程不安全的, 存在一定的可能性输出两个Done
            _done = false;
            new Thread(Go2).Start();
            Go2();
        }

        static void Go1()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.Write("?");
            }
        }
        static void Go2()
        {
            if (!_done)
            {
                _done = true;
                Console.WriteLine("Done");
            }
        }

        #endregion

        #region 锁和线程安全

        public static object _locker;

        /// <summary>
        /// 锁和线程安全
        /// </summary>
        [Test]
        public void ThreadSafetyTest()
        {
            //在不确定的多线程上下文下,采用锁的方式进行代码保护的代码称为线程安全的代码
            //当我们在读写共享变量时可以首先获得一个排他锁来保证线程安全
            _done = false;
            new Thread(Go2).Start();
            Go2();
        }

        static void Go3()
        {
            lock (_locker)
            {
                if (!_done)
                {
                    _done = true;
                    Console.WriteLine("Done");
                }
            }
        }

        #endregion

        #region 向线程传递数据

        /// <summary>
        /// 向线程传递数据
        /// </summary>
        [Test]
        public void PassOnDataToThreadTest()
        {
            //方法1 使用Lambda表达式
            var message = "Hello form t1"; 
            var t1 = new Thread(() =>
             {
                 ThreadPrint(message);
             });
            t1.Start();
            //在使用Lambda表达式的变量捕获时要注意, 线程开始后就尽量不要修改变量
            //如果修改变量,会导致输出不确定, 例如下例无法输出0-9
            /*for (int i = 0; i < 10; i++)
            {
                new Thread(() => { ThreadPrint(i.ToString()); }).Start();
            }*/
            //若要解决这个问题, 可以在循环内定义一个局部变量
            for (int i = 0; i < 10; i++)
            {
                var temp = i;
                new Thread(() =>
                {
                    ThreadPrint(temp.ToString());
                }).Start();
            }

            //方法2 使用委托ParameterizedTreadStart
            //缺点: 委托ParameterizedTreadStart只有一个参数, 且需要类型转换
            var t2 = new Thread(ParameterizedTreadStart);
            t2.Start(new Person { Age = 1, FirstName = "AAA" });
            Console.WriteLine("main:" + Thread.CurrentThread.ManagedThreadId);
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

        public void ThreadPrint(string message)
        {
            Console.WriteLine(message);
        }

        #endregion

        #region 前台线程和后台线程

        /// <summary>
        /// 前台线程和后台线程
        /// </summary>
        [Test]
        public void BackgroundThreadTest()
        {
            //前台线程:显式创建的线程默认就是前台线程
            //后台线程:将Thread.IsBackground置为true的线程
            //前/后台线程和线程优先级无关
            //进程中只要有一个前台线程在运行，那么进程就处于激活状态
            var t = new Thread(RunMain2)
            {
                IsBackground = true,
                Name = "BackgroundThreadTest",
            };
            t.Start();
            Console.WriteLine("Main thread ending now");
            //后台线程在进程终止时便终止了, 这时后台线程的执行栈上的任何finally语句块都无法执行
        }

        public void RunMain2()
        {
            Console.WriteLine("ThreadMain3Start");
            Thread.Sleep(3000);
            Console.WriteLine("ThreadMain3End");
        }

        #endregion

        #region 线程优先级

        /// <summary>
        /// 线程优先级
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
        }

        #endregion

        #region 信号发送

        /// <summary>
        /// 信号发送
        /// </summary>
        [Test]
        public void ThreadSignalingTest()
        {
            //信号发送:一个线程等待来着其他线程的通知

            //ManualResetEvent是最简单的信号发送结构
            var signal = new ManualResetEvent(false);

            new Thread(() =>
            {
                Console.WriteLine("Waiting for signal");

                //WaitOne会阻塞线程
                signal.WaitOne();

                //
                signal.Dispose();

                Console.WriteLine("Got signal");
            }).Start();

            Thread.Sleep(2000);

            //Set会打开被signal阻塞的线程, 使用Reset可以将其再次关闭
            signal.Set();
        }

        #endregion
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
}