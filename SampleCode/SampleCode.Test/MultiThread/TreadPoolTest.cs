using System;
using System.IO;
using System.Text;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.MultiThread
{
    /// <summary>
    /// 线程池
    /// 线程池中所有的线程默认为后台线程，优先级为Normal 
    /// </summary>
    [TestFixture]
    public class TreadPoolTest
    {
        /// <summary>
        /// 基本用法
        /// </summary>
        [Test]
        public void TreadPoolCodeTest()
        {
            //获取最大工作线程和最大I/O线程的数量
            ThreadPool.GetMaxThreads(out int nWorkerThreads, out int nCompletionPortThreads);

            //设置最大线程
            ThreadPool.SetMaxThreads(1100, 1100);

            //设置最少保留线程
            ThreadPool.SetMinThreads(100, 100);

            for (var i = 0; i < 5; i++)
            {
                //传入QueueUserWorkItem的方法需要实现委托WaitCallback
                ThreadPool.QueueUserWorkItem(JobForAThread);

                //获取可用线程
                ThreadPool.GetAvailableThreads(out nWorkerThreads, out nCompletionPortThreads);
                Console.WriteLine($"Available WorkTread:{nWorkerThreads}, I/O Tread:{nCompletionPortThreads}");
            }

            Console.Read();
        }

        /// <summary>
        /// 实现委托WaitCallback的方法
        /// </summary>
        /// <param name="state"></param>
        private static void JobForAThread(object state)
        {
            for (var i = 0; i < 3; i++)
            {
                Console.WriteLine($"loop:{i}, TreadId:{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// 利用ThreadPool调用工作线程和IO线程的范例
        /// 工作线程是指在CLR中运作的线程，主要进行计算密集的任务
        /// IO线程是指与外部系统交互信息的线程
        /// </summary>
        [Test]
        public void TreadPoolTest2()
        {
            ThreadPool.GetMaxThreads(out int nWorkerThreads, out int nCompletionPortThreads);
            Console.WriteLine($"最大工作线程为：{nWorkerThreads},最大IO线程为：{nCompletionPortThreads}");
            PrintThreadMessage("Main Thread Start");

            //Thread.CurrentThread.Join();

            //调用工作线程
            ThreadPool.QueueUserWorkItem(AsyncMethod);

            var stream = new FileStream(@"D:\Desktop\1.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite,
                1024, true);
            //这里要注意，如果写入的字符串很小，则.Net会使用辅助线程(后台线程)写，因为这样比较快
            var bytes = Encoding.UTF8.GetBytes("你在他乡还好吗？");
            //异步写入开始，倒数第二个参数指定回调函数，最后一个参数将自身传到回调函数里，用于结束异步线程
            stream.BeginWrite(bytes, 0, bytes.Length, Callback, stream);
            PrintThreadMessage("AsyncReadFile Method");
        }

        public static void AsyncMethod(object obj)
        {
            Thread.Sleep(2000);
            PrintThreadMessage("Asynchoronous Method");
            Console.WriteLine("Asynchoronous thread has worked ");
        }

        public static void Callback(IAsyncResult result)
        {
            //显示线程池现状
            Thread.Sleep(2000);
            PrintThreadMessage("AsyncWriteFile Callback Method");
            //通过result.AsyncState再强制转换为FileStream就能够获取FileStream对象，用于结束异步写入
            var stream = (FileStream)result.AsyncState;
            stream.EndWrite(result);
            stream.Flush();
            stream.Close();
        }

        public static void PrintThreadMessage(string data)
        {
            ThreadPool.GetAvailableThreads(out int nWorkerThreads, out int nCompletionPortThreads);
            Console.WriteLine($@"-------------------------------------------------------------------------");
            Console.WriteLine($@"{data}");
            Console.WriteLine($@"ThreadId:{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($@"IsBackground:{Thread.CurrentThread.IsBackground}");
            Console.WriteLine($"当前最大工作线程为：{nWorkerThreads},最大IO线程为：{nCompletionPortThreads}");
        }
    }
}