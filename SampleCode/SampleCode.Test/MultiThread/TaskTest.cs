using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.MultiThread
{
    /// <summary>
    /// 任务
    /// </summary>
    [TestFixture]
    public class TaskTest
    {
        #region 启动任务

        /// <summary>
        /// 启动任务
        /// </summary>
        [Test]
        public void RunTaskTest()
        {
            //任务默认使用线程池中的线程, 因此都是后台线程
            //但是也可以通过传递TaskCreationOptions.LongRunning参数请求一个非线程池线程

            //方法一: Task构造器
            var task1 = new Task<int>(x => Sum((int)x), 10000);
            task1.Start();
            //Wait方法阻塞当前线程, 和Thread.Join类似
            task1.Wait();
            Console.WriteLine($"task1结果：{task1.Result}");

            //方法二: Task工厂
            var task2 = Task.Factory.StartNew(StartCode, 3);

            //方法三: Task静态方法
            var task3 = Task.Run(() => Console.WriteLine("Foo"));

            //任务的状态 
            //任务的枚举为TaskStatus
            if (task2.Status == TaskStatus.RanToCompletion)
            {
                Console.WriteLine($"task2 is RanToCompletion");
            }
            task2.Wait();

            //Task只提供几个只读Boolean属性：IsCanceled，IsFaulted，IsCompleted，
            //它们能返回最终状态true /false。
            if (task2.IsCompleted)
            {
                Console.WriteLine($"task2 IsCompleted is True");
            }
        }

        private static void StartCode(object i)
        {
            Console.WriteLine($"开始执行任务-{i}");
            Thread.Sleep(1000);
        }


        #endregion

        #region 异常

        /// <summary>
        /// 异常
        /// </summary>
        [Test]
        public void TaskExceptionTest()
        {
            //如果任务中抛出一个未处理的异常, 那么调用Wait或者访问时, 异常会被重新抛出(其中异常会被包装为AggregateException)
            var task1 = Task.Run(() => { throw new ArgumentNullException(); });
            try
            {
                task1.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.Message);
            }

            //使用Task的IsFaulted和IsCanceled属性可以在不抛出异常的情况下检测出错的任务
            //如果IsCanceled为true则说明任务已取消, 抛出了OperationCanceledException
            //如果IsFaulted为true则说明任务抛出了其他异常
            var isFaulted = task1.IsFaulted;

            //异常和自治的任务
            //自治任务指的是那些不需要调用Wait()或访问其Result, 也不需要进行延续的任务
            //在自治任务中抛出的异常称为被称为未观测异常
            //可以使用TaskScheduler.UnobservedTaskException在全局范围订阅未观测异常
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {

            };

        }

        #endregion

        #region 延续

        /// <summary>
        /// 延续
        /// </summary>
        [Test]
        public void TaskContinueTest()
        {
            //延续是指通过回调方法的形式告诉任务在完成之后需要继续执行的任务

            //方式一 Awaiter
            var task1 = Task.Run(() => Enumerable.Range(2, 3000000).Count(x =>
                Enumerable.Range(2, (int)System.Math.Sqrt(x) - 1).All(y => x % y > 0)));

            var awaiter = task1.GetAwaiter();
            //OnCompleted告知先导任务当它执行完毕(或抛出异常)时调用一个委托
            awaiter.OnCompleted(() =>
            {
                //如果先导任务抛出异常, 则awaiter.GetResult()也会将异常重新抛出
                //重新抛出的异常不会被AggregateException包装
                //对于非泛型的任务, GetResult的返回值为void
                var result = awaiter.GetResult();
                Console.WriteLine(result);
                Console.WriteLine("----------");
            });

            task1.Wait();

            //方式二 ContinueWith
            //ContinueWith会返回一个Task, 非常适合添加复数的延续
            //如果希望延续任务和先导任务在同一线程上执行, 需要指定TaskContinuationOptions.ExecuteSynchronously
            var task2 = new Task<int>(x => Sum((int)x), 10000);

            //正常运行时走这个
            task2.ContinueWith(x => Console.WriteLine($"The result is {x.Result}"),
                TaskContinuationOptions.OnlyOnRanToCompletion);

            //报错时走这个
            task2.ContinueWith(x => Console.WriteLine($"result Throw {x.Exception}"),
                TaskContinuationOptions.OnlyOnFaulted);

            //取消时走这个
            task2.ContinueWith(x => Console.WriteLine($"result was cancel {x.IsCanceled}"),
                TaskContinuationOptions.OnlyOnCanceled);

            try
            {
                task2.Start();
            }
            catch (AggregateException e)
            {
                Console.WriteLine("出错");
                throw;
            }
        }

        #endregion

        #region TaskCompletionSource类

        /// <summary>
        /// TaskCompletionSource类
        /// </summary>
        [Test]
        public void TaskCompletionSourceTest()
        {
            //TaskCompletionSource可以创建一个任务, 这个任务不是那种需要执行启动操作的任务,
            //而是操作结束或出错时手动创建的附属任务

            //下例使用TaskCompletionSource编写Run函数
            //本质上下述代码和调用Task.Factory.StartNew, 并传递TaskCreationOptions.LongRunning参数是等价的
            var task = Run((() =>
            {
                Thread.Sleep(1000);
                return 42;
            }));
            task.Wait();
            var result = task.Result;

            //TaskCompletionSource的真正作用在于创建一个不绑定线程的任务
            //例如我们可以使用Timer实现Thread.Delay方法(Timer不是一个线程)
            Delay(1000).GetAwaiter().OnCompleted(() => { Console.WriteLine(42); });

            Thread.Sleep(1500);
        }

        private Task<T> Run<T>(Func<T> function)
        {
            var tcs = new TaskCompletionSource<T>();
            new Thread((() =>
            {
                try
                {
                    tcs.SetResult(function());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            })).Start();

            return tcs.Task;
        }

        public Task Delay(int milliseconds)
        {
            var tcs = new TaskCompletionSource<object>();
            var timer = new System.Timers.Timer(milliseconds) { AutoReset = false };
            timer.Elapsed += delegate
            {
                timer.Dispose();
                tcs.SetResult(null);
            };
            timer.Start();

            return tcs.Task;
        }

        #endregion

        #region Task.Delay方法

        /// <summary>
        /// Task.Delay方法
        /// </summary>
        [Test]
        public void DelayTaskTest()
        {
            //Task.Delay是异步版本的Thread.Sleep
            Task.Delay(1000).GetAwaiter().OnCompleted(() => {Console.WriteLine("1");});

            Thread.Sleep(1500);
        }

        #endregion

        /// <summary>
        /// 子任务
        /// </summary>
        [Test]
        public void TaskChildTest()
        {
            var parent = new Task<int[]>(() =>
            {
                var results = new int[3];
                //AttachedToParent将一个Task和创建它的那个Task关联起来，
                //除非所有子任务（子任务的子任务）结束运行，否则创建任务（父任务）不会认为已经结束
                new Task(() => results[0] = Sum(1000), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = Sum(10000), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = Sum(100), TaskCreationOptions.AttachedToParent).Start();
                return results;
            });

            var cwt = parent.ContinueWith(x => Array.ForEach(x.Result, Console.WriteLine));
            parent.Start();
            cwt.Wait();
        }

        /// <summary>
        /// 任务工厂
        /// 使用条件：
        /// 1.需要创建一组Task对象来共享相同的状态
        /// 2.为了避免机械的将相同的参数传给每一个Task的构造器
        /// </summary>
        [Test]
        public void TaskFactory()
        {
            var parent = new Task(() =>
            {
                var cts = new CancellationTokenSource();
                var tf = new TaskFactory<int>(cts.Token, TaskCreationOptions.AttachedToParent,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                //AttachedToParent将一个Task和创建它的那个Task关联起来，
                //除非所有子任务（子任务的子任务）结束运行，否则创建任务（父任务）不会认为已经结束
                var childTasks = new[]
                {
                    tf.StartNew(() => Sum(cts.Token, 10000), cts.Token),
                    tf.StartNew(() => Sum(cts.Token, 20000), cts.Token),
                    tf.StartNew(() => Sum(cts.Token, int.MaxValue), cts.Token),
                };

                //任何子任务抛出异常就取消其余子任务
                foreach (var childTask in childTasks)
                {
                    childTask.ContinueWith(t => cts.Cancel(), TaskContinuationOptions.OnlyOnFaulted);
                }

                // 所有子任务完成后，从未出错/未取消的任务获取返回的最大值
                // 然后将最大值传给另一个任务来显示最大结果
                tf.ContinueWhenAll(childTasks,
                    completedTasks => completedTasks.Where(t => !t.IsFaulted && !t.IsCanceled).Max(t => t.Result),
                    CancellationToken.None)
                    .ContinueWith(t => Console.WriteLine("The maxinum is: " + t.Result), TaskContinuationOptions.ExecuteSynchronously)
                    .Wait(cts.Token); // Wait用于测试
            });

            parent.ContinueWith(x =>
            {
                var sb = new StringBuilder("The following exception(s) occurred:" + Environment.NewLine);
                if (x.Exception == null) return;
                foreach (var innerException in x.Exception.Flatten().InnerExceptions)
                {
                    sb.AppendLine("   " + innerException.GetType().ToString());
                }
                Console.WriteLine(sb.ToString());
            }, TaskContinuationOptions.OnlyOnFaulted);

            parent.Start();

            try
            {
                parent.Wait();
            }
            catch (AggregateException e)
            {

            }
        }

        /// <summary>
        /// 取消和暂停任务
        /// </summary>
        [Test]
        public void TaskCancelAndStopTest()
        {

            // 取消标记源
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            // ManualResetEvent是一线程用来控制别一个线程的信号。
            var resetEvent = new ManualResetEvent(true);

            var task = new Task(async () =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    // 初始化为true时执行WaitOne不阻塞
                    resetEvent.WaitOne();

                    // 模拟等待100ms, 等效于Thread.Sleep
                    await Task.Delay(100, token);
                }
            }, token);

            //任务暂停
            resetEvent.Reset();
            //任务继续
            resetEvent.Set();
            //任务取消
            tokenSource.Cancel();
        }
        private static int Sum(int i)
        {
            var sum = 0;
            for (; i > 0; i--)
            {
                //用于打开整数溢出检查
                //与之相反的是unchecked，用于关闭整数溢出检查
                //默认是关闭状态
                checked
                {
                    sum += i;
                }
            }
            return sum;
        }

        private static int Sum(CancellationToken ct, int i)
        {
            var sum = 0;
            for (; i > 0; i--)
            {
                ct.ThrowIfCancellationRequested();

                //用于打开整数溢出检查
                //与之相反的是unchecked，用于关闭整数溢出检查
                //默认是关闭状态
                checked
                {
                    sum += i;
                }
            }
            return sum;
        }
    }
}