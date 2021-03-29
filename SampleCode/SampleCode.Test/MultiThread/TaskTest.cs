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
        #region 基于任务的异步模式原则

        /*
         * 基于任务的异步模式(Task-based Asynchronous Pattern, TAP)
         * 1.返回一个正在运行的Task或者Task<TResult>
         * 2.拥有async后缀(除了任务组合器等特殊情况)
         * 3.若支持取消操作或进度报告, 则需要有接受CancellationToken或IProgress的重载
         * 4.快速返回调用者
         * 5.对于I/O密集操作不绑定线程
         */

        #endregion

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
            var task1 = Task.Run(() => throw new ArgumentNullException());
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
                args.SetObserved();
                ((AggregateException)args.Exception)?.Handle(ex =>
                {
                    Console.WriteLine("Exception type: {0}", ex.GetType());
                    return true;
                });
            };

            Run();

            Thread.Sleep(100);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private static void Run()
        {
            var task1 = new Task(() => throw new ArgumentNullException());
            var task2 = new Task(() => throw new ArgumentOutOfRangeException());

            task1.Start();
            task2.Start();

            while (!task1.IsCompleted || !task2.IsCompleted)
            {
                Thread.Sleep(50);
            }
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
            //TaskCompletionSource是一种在底层实现I/O密集异步方法的标准手段

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
            Console.WriteLine("TaskCompletionSourceTest:" + Thread.CurrentThread.ManagedThreadId);
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
                Console.WriteLine("timer:" + Thread.CurrentThread.ManagedThreadId);
            };
            timer.Start();

            Console.WriteLine("Delay:" + Thread.CurrentThread.ManagedThreadId);

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
            //不同点
            //Sleep不占用CPU处理资源
            //Delay会占用CPU处理资源, 只是延迟了几秒执行代码而已
            Task.Delay(1000).GetAwaiter().OnCompleted(() => { Console.WriteLine("1"); });

            Thread.Sleep(1500);
        }

        #endregion

        #region 取消任务

        /// <summary>
        /// 取消任务
        /// </summary>
        [Test]
        public void TaskCancelTest()
        {
            //并发操作在启动后都可以通过取消标记源取消相关操作
            //CLR中大部分异步方法都支持取消令牌

            //取消标记源
            //CancellationTokenSource可以在构造函数中指定一个时间间隔,
            //达到在一段时间之后启动取消操作的目的
            var tokenSource = new CancellationTokenSource();
            //取消令牌
            var token = tokenSource.Token;
            //CancellationToken可以注册一个在取消操作发生时的回调函数
            token.Register(() =>
            {
                Console.WriteLine("Task cancel");
            });

            var task = Task.Run(async () =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        //也可以直接抛出ThrowIfCancellationRequested
                        //token.ThrowIfCancellationRequested();
                        return;
                    }

                    //模拟等待100ms, 等效于Thread.Sleep
                    await Task.Delay(100, token);
                }
            }, token);

            //任务取消
            tokenSource.Cancel();
        }

        #endregion

        #region 进度报告

        /// <summary>
        /// 进度报告
        /// </summary>
        [Test]
        public async Task TaskProgressTest()
        {
            //CLR专门针对进度报告的类型: IProgress<T>接口和Progress类
            //类Progress构造函数中可以接受一个委托对进度进行包装
            var onProgressPercentChanged = new Progress<int>(x => Console.WriteLine(x + "%"));
            await Foo(onProgressPercentChanged);

        }

        public Task Foo(IProgress<int> onProgressPercentChanged)
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (i % 10 == 0)
                    {
                        onProgressPercentChanged.Report(i / 10);

                        Thread.Sleep(10);
                        //逻辑
                    }
                }
            });
        }

        #endregion

        #region 任务组合器

        /// <summary>
        /// 任务组合器
        /// </summary>
        [Test]
        public async Task CombinationTaskTest()
        {
            //CLR包含两种任务组合器Task.WhenAny和Task.WhenAll

            //Task.WhenAny方法会在任务组中任意一个任务完成时返回这个任务
            //Task.WhenAny会返回第一个完成的任务
            //如果某个非第一个结束的任务出现了异常, 除非我们等待这个任务, 否则这个异常将成为未观测的异常
            var winningTask = await Task.WhenAny(Delay1(), Delay2(), Delay3());
            Console.WriteLine(winningTask.Result);  //1
            //Task.WhenAny可以在原本不支持超时和取消的操作中添加超时和取消功能
            var task = SomeAsyncFunc();
            Task winner = await Task.WhenAny(task, Task.Delay(5000));
            if (task != winner)
            {
                //throw new TimeoutException();
                Console.WriteLine("TimeoutException");
            }

            //Task.WhenAll返回一个任务, 该任务仅当参数中所有任务全部完成时才完成
            //等价于各个任务分别await调用, 但是复数次等待的效率一般是低于一次等待的
            var allResult = await Task.WhenAll(Delay1(), Delay2(), Delay3());
            foreach (var i in allResult)
            {
                Console.WriteLine(i);
            }
            //如果组合任务产生了复数个异常, 则可以通过以下写法获取所有异常,
            //这些异常组合在AggregateException中
            var task1 = Task.Run(() => throw new ArgumentNullException());
            var task2 = Task.Run(() => throw new NullReferenceException());
            var all = Task.WhenAll(task1, task2);
            try
            {
                await all;
            }
            catch (Exception e)
            {
                Console.WriteLine(all.Exception.InnerExceptions.Count); //2
            }

            //自定义组合器
            //1.为任一任务设定一个超时时间
            var timeoutTask = Task.Run(async () =>
            {
                await Task.Delay(1000);
                return 1;
            }).WithTimeout(new TimeSpan(0, 0, 1));
        }

        private async Task<int> Delay1() { await Task.Delay(1000); return 1; }
        private async Task<int> Delay2() { await Task.Delay(2000); return 2; }
        private async Task<int> Delay3() { await Task.Delay(3000); return 3; }

        private async Task<string> SomeAsyncFunc() { await Task.Delay(10); return "SomeAsyncFunc"; }

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
    public static class TaskUtil
    {
        public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
        {
            Task winner = Task.WhenAny(task, Task.Delay(timeout));
            if (winner != task)
            {
                throw new TimeoutException();
            }

            return await task;
        }
    }
}