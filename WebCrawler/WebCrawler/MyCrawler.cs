﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.Events;

namespace WebCrawler
{
    class MyCrawler : ICrawler
    {
        public event EventHandler<OnStartEventArgs> OnStart;
        public event EventHandler<OnCompletedEventArgs> OnCompleted;
        public event EventHandler<OnErrorEventArgs> OnError;

        public CookieContainer CookieContainer { get; set; }

        public MyCrawler() { }

        public async Task<string> Start(Uri uri, string refererUrl, Dictionary<string, string> specialArguments = null)
        {
            return await Task.Run(() =>
            {
                OnStart?.Invoke(this, new OnStartEventArgs(uri));
                var pageSource = string.Empty;
                Bitmap bitmap = new Bitmap(100,200);
                try
                {
                    var sw = new Stopwatch();
                    sw.Start();

                    //设置报文/创建连接
                    var request = WebRequest.Create(uri) as HttpWebRequest;
                    request.Accept = "*/*";
                    request.ServicePoint.Expect100Continue = false;
                    request.ServicePoint.UseNagleAlgorithm = false;
                    request.ServicePoint.ConnectionLimit = int.MaxValue;
                    request.AllowAutoRedirect = false;
                    request.AllowWriteStreamBuffering = false;
                    request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gizp,deflate");
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
                    request.Timeout = 5000;
                    request.KeepAlive = true;
                    request.Method = "Get";
                    request.Referer = refererUrl;
                    request.CookieContainer = CookieContainer;
                    if(specialArguments != null && specialArguments["proxy"] != null) request.Proxy = new WebProxy(specialArguments["proxy"]);

                    using (var response = (HttpWebResponse) request.GetResponse())
                    {
                        var responseStream = response.GetResponseStream();

                        //将cookie加入cookie容器
                        foreach (Cookie cookie in response.Cookies)
                        {
                            CookieContainer.Add(cookie);
                        }

                        if (response.ContentEncoding.ToLower().Equals("gizp"))
                        {
                            using (var stream = new GZipStream(responseStream, CompressionMode.Decompress))
                            {
                                using (var reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                        else if (response.ContentEncoding.ToLower().Equals("deflate"))
                        {
                            using (var stream = new DeflateStream(responseStream, CompressionMode.Decompress))
                            {
                                using (var reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                        else if (response.ContentEncoding.ToLower().Equals("image/jpeg"))
                        {
                            using (var stream = new MemoryStream())
                            {
                                responseStream.CopyTo(stream);
                                bitmap = new Bitmap(stream);
                            }
                        }
                        else
                        {
                            using (Stream stream = response.GetResponseStream())//原始
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                    }

                    request.Abort(); //取消请求
                    sw.Stop();
                    var milliseconds = sw.ElapsedMilliseconds;//获取请求执行时间
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    OnCompleted?.Invoke(this,new OnCompletedEventArgs(uri, threadId, milliseconds, pageSource, bitmap, specialArguments));
                }
                catch (Exception e)
                {
                    OnError?.Invoke(this, new OnErrorEventArgs(uri, e));
                }

                return string.Empty;
            });
        }
    }

}
