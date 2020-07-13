using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.Diagnosis
{
    /// <summary>
    /// 性能计数器
    /// </summary>
    [TestFixture]
    public class PerformanceCounterCategoryTest
    {
        /*
         * 性能计数器是用于监视基础结构，它是由一系列系统或应用程序公开的
         * 性能计数器按照"System(系统)"、"Processor(处理器)"、".NET CLR Memory(.NET CLR内存)"等类别分组
         */

        #region 获取所有可用的性能计数器

        /// <summary>
        /// 获取所有可用的性能计数器
        /// </summary>
        [Test]
        public void GetAllPerformanceCounterCategoryTest()
        {
            //运行速度较慢，因为输出将超过1万行
            var cats = PerformanceCounterCategory.GetCategories();

            foreach (var cat in cats)
            {
                //获取所有类别
                Console.WriteLine("Category:" + cat.CategoryName);

                //获取所有实例
                var instances = cat.GetInstanceNames();

                //获取计数器
                if (instances.Length == 0)
                {
                    foreach (var counter in cat.GetCounters())
                    {
                        Console.WriteLine("  Counter:" + counter.CounterName);
                    }
                }
                else
                {
                    foreach (var instance in instances)
                    {
                        Console.WriteLine("  Instance:" + instance);
                        if (cat.InstanceExists(instance))
                        {
                            foreach (var counter in cat.GetCounters(instance))
                            {
                                Console.WriteLine("    Counter:" + counter.CounterName);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 检索性能计数器

        /// <summary>
        /// 检索性能计数器
        /// </summary>
        public void MonitorTest()
        {
            var stopper = new ManualResetEvent(false);

            //监视处理器时间
            new Thread(() =>
            {
                Monitor("Processor", "% Processor Time", "_Total", stopper);
            }).Start();

            //监视硬盘空闲时间
            new Thread(() =>
            {
                Monitor("LogicalDisk", "% Idle Time", "C:", stopper);
            }).Start();
        }

        public static void Monitor(string categroy, string counter, string instance, EventWaitHandle stopper)
        {
            if (!PerformanceCounterCategory.Exists(categroy))
                throw new InvalidOperationException("Categroy does not exist");
            if (!PerformanceCounterCategory.CounterExists(counter, categroy))
                throw new InvalidOperationException("Counter does not exist");

            instance ??= "";
            if (instance != "" && !PerformanceCounterCategory.InstanceExists(instance, categroy))
                throw new InvalidOperationException("Instance does not exist");

            var lastValue = 0f;

            using (var pc = new PerformanceCounter(categroy, counter, instance))
            {
                //每200毫秒轮询一次
                while (!stopper.WaitOne(200, false))
                {
                    var value = pc.NextValue();
                    if (System.Math.Abs(value - lastValue) > 0)
                    {
                        Console.WriteLine(value);
                        lastValue = value;
                    }
                }
            }

        }

        #endregion
    }
}