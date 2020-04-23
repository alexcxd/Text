using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.MultiThread
{
    /// <summary>
    /// Parallel主要用于任务的并行
    /// Parallel类定义了并行for和foreach的静态方法
    /// 在Parallel中的执行是没有顺序的
    /// </summary>
    [TestFixture]
    public class ParallelTest
    {
        /// <summary>
        /// Parallel基本用法
        /// </summary>
        [Test]
        public void ParallelCodeTest()
        {
            //Invoke基本用法
            //用来并行方法
            var actions = new Action[]
            {
                () => ActionMethod("test 1"),
                () => ActionMethod("test 2"),
                () => ActionMethod("test 3"),
                () => ActionMethod("test 4")
            };
            Parallel.Invoke(actions);

            Console.WriteLine("----------------------------------------------");
            Thread.Sleep(100);

            //for的基本用法
            //可以通过ParallelLoopState(pls)使循环结束
            var result = Parallel.For(0, 10, (i, pls) =>
            {
                Console.WriteLine($"{i} task:{Task.CurrentId} thread:{Thread.CurrentThread.ManagedThreadId}");

                if (i >= 7)
                {
                    pls.Break();
                }
            });

            Console.WriteLine("----------------------------------------------");
            Thread.Sleep(100);

            //foreach的基本用法
            var nums = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            Parallel.ForEach(nums, (item) =>
            {
                Console.WriteLine($"{item} task:{Task.CurrentId} thread:{Thread.CurrentThread.ManagedThreadId}");
            });
        }

        private static void ActionMethod(object value) =>
            Console.WriteLine($">>> thread:{Thread.CurrentThread.ManagedThreadId}, value:{value}");

        /// <summary>
        /// 通过Foreach实现数据分区
        /// 分区采用Partitioner.Create实现
        /// </summary>
        [Test]
        public void ParallelDataPartitionTest()
        {
            for (var j = 1; j < 4; j++)
            {
                // 表示对象的线程安全的无序集合。
                var bag = new ConcurrentBag<int>();
                var sw = Stopwatch.StartNew();
                sw.Start();

                // Partitioner提供针对数组、列表和可枚举项的常见分区策略。
                var tuples = Partitioner.Create(0, 3000000);
                Parallel.ForEach(tuples, i =>
                {
                    var (item1, item2) = i;
                    Console.WriteLine($"item1:{item1} item2:{item2}");
                    for (var m = item1; m < item2; m++)
                    {
                        bag.Add(m);
                    }
                });
                Console.WriteLine($"并行计算：集合有:{bag.Count},总共耗时：{sw.ElapsedMilliseconds}");
                GC.Collect();
            }
        }

        /// <summary>
        /// ParallelOptions
        /// 用于配置一些并行相关的设置
        /// </summary>
        [Test]
        public void ParallelOptionsTest()
        {
            var sw = Stopwatch.StartNew();
            var dic = new ConcurrentDictionary<int, string>();
            var taskIds = new ConcurrentBag<int>();
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 4, //设置最大并行数
                
            };
            //预加载1500w条记录
            Parallel.For(0, 15000000, options, (i) =>
            {
                var single = $"string{i}";
                dic.TryAdd(i, single);
            });
            Console.WriteLine($"time:{sw.ElapsedMilliseconds}");
        }

        /// <summary>
        /// Parallel退出循环
        /// </summary>
        [Test]
        public void ParallelExitTest()
        {
            var bag = new ConcurrentBag<int>();

            Parallel.For(0, 20000, (i, state) =>
            {
                if (i > 1000)
                {
                    //state.Break();    //通知并行计算尽快的退出循环，例如大于100是退出循环，break后程序还会迭代所有大于100的。
                    state.Stop();       //直接退出循环，不管后续线程
                    return;
                }
                bag.Add(i);
            });

            Console.WriteLine($"bag count:{bag.Count}");
        }

        /// <summary>
        /// Parallel取消
        /// </summary>
        [Test]
        public void ParallelCancelTest()
        {
            var cts = new CancellationTokenSource();
            cts.Token.Register(() => Console.WriteLine($"Thread{Thread.CurrentThread.ManagedThreadId} Cancel"));

            Task.Factory.StartNew(() =>
            {
                Parallel.For(0, 100,
                    new ParallelOptions {  CancellationToken = cts.Token },
                    i =>
                    {
                        Console.WriteLine($"i:{i} ThreadId:{Thread.CurrentThread.ManagedThreadId} TaskId:{Task.CurrentId}");
                    });
            }, cts.Token);

            Thread.Sleep(2);

            cts.Cancel();
        }

        /// <summary>
        /// Parallel的异常处理
        /// </summary>
        [Test]
        public void ParallelExceptionTest()
        {
            try
            {
                Parallel.Invoke(Run1, Run2);
            }
            catch (AggregateException ex)
            {
                foreach (var single in ex.InnerExceptions)
                {
                    Console.WriteLine(single.Message);
                }
            }
        }

        private static void Run1()
        {
            Thread.Sleep(3000);
            throw new Exception("Run1 Exception");
        }

        private static void Run2()
        {
            Thread.Sleep(5000);
            throw new Exception("Run2 Exception");
        }
    }
}