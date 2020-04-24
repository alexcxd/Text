using System;
using System.Threading;
using NUnit.Framework;

namespace SampleCode.Test.MultiThread
{
    /// <summary>
    /// 线程同步
    /// </summary>
    [TestFixture]
    public class ThreadSynchronousTest
    {
        private static int _flag;
        private static readonly object Locker = new object();
        private static AutoResetEvent _resetEvent = new AutoResetEvent(false);

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

        /// <summary>
        /// lock确保当一个线程位于代码的临界区时，另一个线程不进入临界区。
        /// 如果其他线程试图进入锁定的代码，则它将一直等待（即被阻止），直到该对象被释放。
        /// 提供给lock的参数必须私有\受保护的引用类型，且不可是字符串
        /// lock是用 Monitor 类来实现的
        /// </summary>
        [Test]
        public void LockTest()
        {
            for (var i = 0; i < 3; i++)
            {
                var thread = new Thread(LockPrinter) { Name = $"thread{i}" };
                Thread.Sleep(100);
                thread.Start();
            }
        }

        private static void LockPrinter()
        {
            lock (Locker)
            {
                Console.WriteLine("{0} acquired the lock", Thread.CurrentThread.Name);
                //模拟打印操作
                Thread.Sleep(500);
                Console.WriteLine("{0} exiting lock", Thread.CurrentThread.Name);
            }
        }

        /// <summary>
        /// Enter 方法允许一个且仅一个线程继续执行后面的语句；
        /// 其他所有线程都将被阻止，直到执行语句的线程调用 Exit。
        /// </summary>
        [Test]
        public void MonitorEnterTest()
        {
            for (var i = 0; i < 3; i++)
            {
                var thread = new Thread(MonitorEnterPrinter) { Name = $"thread{i}" };
                Thread.Sleep(100);
                thread.Start();
            }
        }
        private static void MonitorEnterPrinter()
        {
            Monitor.Enter(Locker);

            try
            {
                Console.WriteLine("{0} acquired the lock", Thread.CurrentThread.Name);
                //模拟打印操作
                Thread.Sleep(500);
                Console.WriteLine("{0} exiting lock", Thread.CurrentThread.Name);
            }
            finally
            {
                Monitor.Exit(Locker);
            }
        }


        /// <summary>
        /// 同步事件,通过信号量同步线程
        /// AutoResetEvent
        /// </summary>
        [Test]
        public void AutoResetEventTest()
        {
            //将事件状态设置为终止状态，从而最多允许一个等待线程继续执行。
            _resetEvent.Set();

            //将事件状态设置为非终止，从而导致线程受阻。
            //_resetEvent.Reset();

            var thread1 = new Thread(AutoResetEventThreadTest)
            {
                Name = "Thread1"
            };
            thread1.Start();
            var thread2 = new Thread(AutoResetEventThreadTest)
            {
                Name = "Thread2"
            }; ;
            thread2.Start();

            Thread.Sleep(10000);
        }

        private static void AutoResetEventThreadTest()
        {
            Console.WriteLine($"Thread: {Thread.CurrentThread.Name} start");
            _resetEvent.WaitOne();
            Console.WriteLine($"Thread: {Thread.CurrentThread.Name} running");
            Thread.Sleep(1000);
            Console.WriteLine($"Thread: {Thread.CurrentThread.Name} over");
            _resetEvent.Set();
        }
    }
}