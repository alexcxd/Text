using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SampleCode.Test.MultiThread
{
    /// <summary>
    /// 线程同步
    /// </summary>
    [TestFixture]
    public class ThreadSynchronousTest
    {
        #region 排他锁

        private static readonly object Locker = new();

        /// <summary>
        /// Lock基本信息
        /// </summary>
        [Test]
        public void LockTest()
        {
            //lock确保当一个线程位于代码的临界区时，另一个线程不进入临界区,
            //如果其他线程试图进入锁定的代码，则它将一直等待（即被阻止），直到该对象被释放
            //lock是包裹在try/finally语句块中的Monitor.Enter和Monitor.Exist的语法糖
            //锁的操作一般是很快的(50ns)

            //同步对象是指一个所有参与线程都可见，且为引用类型的对象可以作为同步对象(这里是Locker)
            //同步对象一般是私有的静态或实例字段
            //在Lambda表达式中匿名方法捕获的局部变量也可作为同步对象

            //这里用任务模拟并发
            var tasks = new List<Task>();

            for (var i = 0; i < 3; i++)
            {
                var action = (Action) delegate
                {
                    lock (Locker)
                    {
                        Console.WriteLine("{0} acquired the lock", Thread.CurrentThread.Name);
                        //模拟打印操作
                        Thread.Sleep(500);
                        Console.WriteLine("{0} exiting lock", Thread.CurrentThread.Name);
                    }
                };

                tasks.Add(Task.Run(action));
            }

            Task.WaitAll(tasks.ToArray());

            //等价于

            #region Monitor.Enter

            //Monitor.Enter方法允许一个且仅一个线程继续执行后面的语句
            //其他所有线程都将被阻止，直到执行语句的线程调用 Exit
            tasks = new List<Task>();

            for (var i = 0; i < 3; i++)
            {
                var action = (Action)delegate
                {
                    var lockTaken = false;

                    try
                    {
                        //C#4.0加入的重载
                        Monitor.Enter(Locker, ref lockTaken);
                        //Monitor提供了TryEnter方法可以设置一个超时时间
                        //Monitor.TryEnter(Locker, 1000, ref lockTaken);

                        Console.WriteLine("{0} acquired the lock", Thread.CurrentThread.Name);
                        //模拟打印操作
                        Thread.Sleep(500);
                        Console.WriteLine("{0} exiting lock", Thread.CurrentThread.Name);
                    }
                    finally
                    {
                        if(lockTaken) Monitor.Exit(Locker);
                    }
                };

                tasks.Add(Task.Run(action));
            }

            Task.WaitAll(tasks.ToArray());

            #endregion

        }

        /// <summary>
        /// Mutex基本信息
        /// </summary>
        [Test]
        public void MutexTest()
        {
            //Mutex在功能上和lock类型, 但是它可以支持多个进程
            //Mutex类的WaitOne方法获得该锁, ReleaseMutex方法释放锁
            //Mutex只能在获得锁的线程上释放锁

            //Mutex的一个常见的用途是一次只能运行一个应用程序实例
            using (var mutex = new Mutex(true, @"Global\SampleCode"))
            {
                if (!mutex.WaitOne(TimeSpan.FromSeconds(3), false))
                {
                    Console.WriteLine("已存在其他程序正在运行");
                    return;
                }

                try
                {
                    Console.WriteLine("运行中");
                }
                finally
                {
                    mutex.ReleaseMutex();
                }

            }
        }

        #endregion

        #region 非排他锁

        /// <summary>
        /// 信号量（Semaphore）
        /// </summary>
        [Test]
        public void SemaphoreTest()
        {
            //信号量允许指定数量的线程进入临界区, 即防止过多的线程同时执行特定代码
            //信号量有两个功能相似的实现: Semaphore 和 SemaphoreSlim
            //信号量没有持有者的概念, 它不需要锁的开启线程释放
            //Semaphore可以进行进程间锁(类似Mutex)
            //SemaphoreSlim比Semaphore执行速度更快(WaitOne和Release方法), 同时它还支持取消令牌

            //实现一次只允许3个线程进入特定代码
            var semaphore = new SemaphoreSlim(3);   //指定初始最大可以进入线程数量
            for (var i = 0; i < 6; i++)
            {
                var id = i;
                Task.Run(() =>
                {
                    Console.WriteLine(id + "想要进入");
                    semaphore.Wait();   //增加已锁定数量, 如果大于最大可进入线程数量, 则等待
                    Console.WriteLine(id + "进入");
                    Thread.Sleep(1000);
                    semaphore.Release();    //减少已锁定数量
                    Console.WriteLine(id + "退出");
                });
            }
        }

        /// <summary>
        /// 读写锁（ReaderWriterLock）
        /// </summary>
        [Test]
        public void ReaderWriterLockTest()
        {
            //读写锁
            var rw = new ReaderWriterLockSlim();
            rw.EnterUpgradeableReadLock();
        }

        #endregion

        #region 信号发送结构



        #endregion

        private static int _flag;

        /// <summary>
        /// Interlocked为多个线程共享的变量提供原子操作。
        /// 即提供以线程安全的方式进行递增、递减、交换和读取值的方法
        /// </summary>
        [Test]
        public void InterlockedTest()
        {
            for (var i = 0; i < 3; i++)
            {
                var thread = new Thread(UsePrinter) { Name = $"thread{i}" };
                Thread.Sleep(200);
                thread.Start();
            }

            var t1 = 1;
            var t2 = 2;
            var t3 = 3;
            //以原子操作的形式，添加两个整数并用两者的和替换第一个整数。
            var result1 = Interlocked.Add(ref t1, t2);
            //比较两个值是否相等，如果相等，则替换第一个值。
            //比较第一个和第三个参数，如果相等将第一个参数替换为第二个
            var result2 = Interlocked.CompareExchange(ref t1, 2, 3);
            //以原子操作的形式递减指定变量的值并存储结果。
            var result3 = Interlocked.Decrement(ref t3);
            //以原子操作的形式递增指定变量的值并存储结果。
            var result4 = Interlocked.Increment(ref t3);
            //以原子操作的形式将变量设置为指定的值。
            var result5 = Interlocked.Exchange(ref t2, 3);
        }

        /// <summary>
        /// 打印机
        /// </summary>
        private static void UsePrinter()
        {
            //检查大引进是否在使用，如果原始值为0，则为未使用，可以进行打印，否则不能打印，继续等待
            //Exchange方法以原子操作的形式将变量设置为指定的值。
            if (0 == Interlocked.Exchange(ref _flag, 1))
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} acquired the lock");

                Thread.Sleep(500);

                Console.WriteLine($"{Thread.CurrentThread.Name} exiting lock");

                //释放打印机
                Interlocked.Exchange(ref _flag, 0);
            }
            else
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} was denied the lock");
            }
        }
    }
}