using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.MultiThread
{
    [TestFixture]
    public class TaskTest
    {
        /// <summary>
        /// 任务基本功能
        /// </summary>
        [Test]
        public void TaskCodeTest()
        {
            //任务的创建和启动
            //方法一：传入Action<object>
            new Task(StartCode, 1).Start();

            //方法二：使用lamada表达式
            var task1 = new Task<int>(x => Sum((int)x), 10000);
            task1.Start();
            task1.Wait();
            Console.WriteLine($"task1结果：{task1.Result}");

            //方法三：使用TaskFactory
            var tf = new TaskFactory();
            tf.StartNew(StartCode, 2);

            //方法四：使用Task.Factory
            var task2 = Task.Factory.StartNew(StartCode, 3);

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

        /// <summary>
        /// 连续的任务
        /// </summary>
        [Test]
        public void TaskContinueWithTest()
        {
            var t = new Task<int>(x => Sum((int)x), 10000);

            //正常运行时走这个
            t.ContinueWith(x => Console.WriteLine($"The result is {x.Result}"),
                TaskContinuationOptions.OnlyOnRanToCompletion);

            //报错时走这个
            t.ContinueWith(x => Console.WriteLine($"result Throw {x.Exception}"), 
                TaskContinuationOptions.OnlyOnFaulted);

            //取消时走这个
            t.ContinueWith(x => Console.WriteLine($"result was cancel {x.IsCanceled}"),
                TaskContinuationOptions.OnlyOnCanceled);

            try
            {
                t.Start();
            }
            catch (AggregateException e)
            {
                Console.WriteLine("出错");
                throw;
            }
        }

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

        private static void StartCode(object i)
        {
            Console.WriteLine($"开始执行任务-{i}");
            Thread.Sleep(1000);
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