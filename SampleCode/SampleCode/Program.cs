using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SampleCode
{
    class Program
    {
        public static void Main(string[] args)
        {
            var semaphore = new SemaphoreSlim(4, 8);   //指定初始最大可以进入线程数量
            for (var i = 0; i < 32; i++)
            {
                var id = i;
                Task.Run(() =>
                {
                    Console.WriteLine(id + "想要进入");
                    semaphore.Wait();
                    Console.WriteLine(id + "进入" + semaphore.CurrentCount);
                    Thread.Sleep(1000);
                    semaphore.Release();
                    Console.WriteLine(id + "退出");
                });
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 测试HttpRequest在高并发情况下的响应时间
        /// </summary>
        public void HttpRequestConcurrentTest()
        {
            //可以同时发出请求的工作线程的数量(不要超过64)
            int numberRequests = 64;

            //用于使主线程保持活动状态，直到所有请求完成
            var manualEvents = new ManualResetEvent[numberRequests];

            //访问的网址
            var httpSite = "http://www.baidu.com";

            //设置默认HTTP并发连接数
            ServicePointManager.DefaultConnectionLimit = 2;
            //ServicePointManager.MaxServicePointIdleTime = 7000;

            //获取线程池的当前设置
            ThreadPool.GetMinThreads(out var minWorker, out var minIoc);

            var sw = new Stopwatch();
            sw.Start();
            //如果没有设置线程池，就会有足够的延迟导致Web请求超时
            ThreadPool.SetMinThreads(64, minIoc);

            #region Parallel并行并限制并发数为4

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 32, //设置最大并行数
            };

            Parallel.For(0, 64, options, (i, pls) =>
            {
                //创建一个类来传递信息给线程
                ThreadStateInfo theInfo = new ThreadStateInfo();

                manualEvents[i] = new ManualResetEvent(false); //线程完成的事件

                theInfo.EvtReset = manualEvents[i];

                theInfo.IReqNumber = i; //跟踪这是什么请求

                theInfo.StrUrl = httpSite; // 要打开的URL

                ThreadProc(theInfo);
            });

            #endregion

            /*for (int i = 0; i < numberRequests; i++)
            {
                //创建一个类来传递信息给线程
                ThreadStateInfo theInfo = new ThreadStateInfo();

                manualEvents[i] = new ManualResetEvent(false); //线程完成的事件

                theInfo.evtReset = manualEvents[i];

                theInfo.iReqNumber = i; //跟踪这是什么请求

                theInfo.strUrl = httpSite; // 要打开的URL

                ThreadPool.QueueUserWorkItem(ThreadProc, theInfo); //让线程池来完成这项工作
            }

            //等待直到ManualResetEvent被设置，这样应用程序在调用回调之后才会退出。
            //这里最多可以等待64个句柄。
            WaitHandle.WaitAll(manualEvents);*/
            sw.Stop();

            Console.WriteLine($"done! Time {sw.ElapsedMilliseconds}");
        }

        // 这个类在工作线程中用于传递信息
        public class ThreadStateInfo
        {
            public string StrUrl; // 我们将发出请求的URL

            public ManualResetEvent EvtReset; // 用来表示这个工作线程已经完成

            public int IReqNumber; // 用于指示这是哪个请求
        }

        // 它完成从工作线程发出http请求的工作
        static void ThreadProc(object stateInfo)
        {
            ThreadStateInfo tsInfo = stateInfo as ThreadStateInfo;

            try
            {
                HttpWebRequest aReq = WebRequest.Create(tsInfo.StrUrl) as HttpWebRequest;

                var servicePoint = aReq.ServicePoint.ConnectionLimit;

                aReq.Timeout = 1000 * 60; 

                Console.WriteLine($"Begin Request {tsInfo.IReqNumber}  {DateTime.Now:yyyy-MM-dd HH:mm:ss:fff}");

                var sw = new Stopwatch();
                sw.Start();
                HttpWebResponse aResp = aReq.GetResponse() as HttpWebResponse;
                sw.Stop();

                var thread = Thread.CurrentThread;

                Console.WriteLine($"Response {tsInfo.IReqNumber} Time {sw.ElapsedMilliseconds}  {DateTime.Now:yyyy-MM-dd HH:mm:ss:fff}");

                aResp.Close(); // 如果不执行此关闭操作，则肯定会超时,套接字将不会被释放。

                Thread.Sleep(500); //模拟服务器处理请求的半秒延迟

                Console.WriteLine("End Request {0}", tsInfo.IReqNumber);
            }
            catch (WebException theEx)
            {
                Console.WriteLine("Exception for Request {0}: {1}", tsInfo.IReqNumber, theEx.Message);
            }

            //通知主线程这个请求已经完成
            tsInfo.EvtReset.Set();
        }
    }
}
