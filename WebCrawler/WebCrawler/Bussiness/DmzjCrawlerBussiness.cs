using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebCrawler.Model;
using WebCrawler.Utils;

namespace WebCrawler.Bussiness
{
    /// <summary>
    /// 爬取动漫之家
    /// </summary>
    public class DmzjCrawlerBussiness
    {
        private static readonly string firstReferer = "https://manhua.dmzj.com/";
        private static readonly string filePath = @"E:\Desktop\漫画";
        private static readonly string filePathLog = filePath + @"\Log.txt";
        private static readonly string picture = "https://images.dmzj.com/h/";
        private static NameToUrlManage manager = new NameToUrlManage();

        public bool GetAllChapters()
        {
            var chapterList = new List<NameToUrl>();
            var uri = firstReferer + @"hjsw/";
            var crawler = new MyCrawler();

            crawler.OnStart += (s, e) =>
            {
                using (var stream = new FileStream(filePathLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine("==================任务一开始=======================");
                        writer.WriteLine(@"地址：" + e.Uri + "开始爬取");
                    }
                }
            };

            crawler.OnError += (s, e) =>
            {
                using (var stream = new FileStream(filePathLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(@"地址：" + e.Uri + "爬取报错; 错误信息：" + e.Exception.Message + "");
                        writer.WriteLine("===============================================");
                    }
                }
            };

            crawler.OnCompleted += (s, e) =>
            {
                using (var stream = new FileStream(filePathLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        var links = Regex.Match(e.PageSource, "cartoon_online_border[^$]{0,}其它汉化版");
                        var urls = Regex.Matches(links.Value,
                            @"<a[^>]+title=""*(?<title>[^>\s]+)""[^>]+href=""*(?<href>[^>\s]+)""\s*[^>]*>[^<]*</a>");

                        var urlQueue = new Queue<NameToUrl>();
                        foreach (Match url in urls)
                        {
                            var nameToUrl = new NameToUrl
                            {
                                Name = url.Groups["title"].Value,
                                Url = url.Groups["href"].Value
                            };
                            urlQueue.Enqueue(nameToUrl);
                        }
                        manager = new NameToUrlManage(urlQueue);

                        writer.WriteLine("爬虫抓取任务完成！开始步骤二！");
                        writer.WriteLine("耗时：" + e.Milliseconds + "毫秒");
                        writer.WriteLine("线程：" + e.ThreadId + "");
                        writer.WriteLine("地址：" + e.Uri.ToString() + "");
                        writer.WriteLine("===============================================");
                        GetChapterId();
                    }
                }

            };
            crawler.Start(new Uri(uri), firstReferer).Wait();

            return true;
        }

        public void GetChapterId()
        {
            var refererUri = firstReferer + @"hjsw/";
            var crawler = new MyCrawler();

            crawler.OnStart += (s, e) =>
            {
                using (var stream = new FileStream(filePathLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine("===================任务二开始======================");
                        writer.WriteLine(@"地址：" + e.Uri + "开始爬取");
                    }
                }

            };

            crawler.OnError += (s, e) =>
            {
                using (var stream = new FileStream(filePathLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(@"地址：" + e.Uri + "爬取报错; 错误信息：" + e.Exception.Message + "");
                        writer.WriteLine("===============================================");
                    }
                }

            };
            crawler.OnCompleted += (s, e) =>
            {
                var url = "";
                using (var stream = new FileStream(filePathLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        var links = Regex.Match(e.PageSource, @"^\|B1_[^|]*\|$");
                        var id = "";
                        if (!String.IsNullOrEmpty(links.Value))
                        {
                            id = links.Value.Substring(3, links.Value.Length - 1);
                        }
                        var name = e.SpecialArguments["Name"].Split('-');
                        url = picture + HttpUtility.UrlEncode(name[0], Encoding.UTF8) + "/" + HttpUtility.UrlEncode(name[1], Encoding.UTF8) + id;
                        
                        writer.WriteLine("爬虫抓取任务完成！开始步骤三！");
                        writer.WriteLine("耗时：" + e.Milliseconds + "毫秒");
                        writer.WriteLine("线程：" + e.ThreadId + "");
                        writer.WriteLine("地址：" + e.Uri.ToString() + "");
                        writer.WriteLine("结果：" + links + name[0] + name[1] + "");
                        writer.WriteLine("===============================================");
                    }
                }
                GetPictures(url, e.Uri.ToString() + "");
            };


            var nameToUrl = manager.GetUrl();
            var url1 = firstReferer + nameToUrl.Url;
            var specialArguments = new Dictionary<string, string>
            {
                { "Name" , $"{nameToUrl.Name}"},
            };
            crawler.Start(new Uri(url1), refererUri, specialArguments);
            /*Parallel.For(0, manager.Count, i =>
            {
                var nameToUrl = manager.GetUrl();
                var url = firstReferer + nameToUrl.Url;
                var specialArguments = new Dictionary<string, string>
                {
                    { "Name" , $"{nameToUrl.Name}"},
                };
                crawler.Start(new Uri(url), refererUri, specialArguments);
            });*/

        }

        public void GetPictures(string uri, string refererUri)
        {
            var crawler = new MyCrawler();

            crawler.OnStart += (s, e) =>
            {
                using (var stream = new FileStream(filePathLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine("===================任务三开始======================");
                        writer.WriteLine(@"地址：" + e.Uri + "开始爬取");
                    }
                }

            };

            crawler.OnError += (s, e) =>
            {
                using (var stream = new FileStream(filePathLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(@"地址：" + e.Uri + "爬取报错; 错误信息：" + e.Exception.Message + "");
                        writer.WriteLine("===============================================");
                    }
                }

            };
            crawler.OnCompleted += (s, e) =>
            {
                using (var stream = new FileStream(filePathLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        var uriStr = e.Uri.ToString().Split('/');
                        var filePathPicture = filePath + "\\" + uriStr[uriStr.Length - 1];
                        var a = Directory.Exists(filePath);
                        e.Bitmap.Save(filePathPicture, ImageFormat.Jpeg);
                        writer.WriteLine("爬虫抓取任务结束！");
                        writer.WriteLine("耗时：" + e.Milliseconds + "毫秒");
                        writer.WriteLine("线程：" + e.ThreadId + "");
                        writer.WriteLine("地址：" + e.Uri.ToString() + "");
                        writer.WriteLine("===============================================");
                    }
                }
            };

            for (int i = 1;; i++)
            {
                var uriTarget = uri + "/" + IntParseString(i) + ".jpg";
                crawler.Start(new Uri(uriTarget), refererUri).Wait();
            }
        }

        public string IntParseString(int num)
        {
            var numStr = num.ToString();
            return numStr;
        }
    }
}