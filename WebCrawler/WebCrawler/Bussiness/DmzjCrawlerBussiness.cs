using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.ClearScript.V8;
using WebCrawler.Crawler;
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
        private static readonly string filePathErrorLog = filePath + @"\ErrorLog.txt";
        private static readonly string pictureMain = "https://images.dmzj.com/";

        public bool GetAllChapters()
        {
            var uri = firstReferer + @"hjsw/";
            var crawler = new MyCrawler();
            var manager = new NameToUrlManage();

            crawler.OnError += (s, e) =>
            {
                using (var stream = new FileStream(filePathErrorLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        writer.WriteLine(@"章节地址爬取失败；地址：" + e.Uri + "爬取报错; 错误信息：" + e.Exception.Message + "");
                        writer.WriteLine("===============================================");
                    }
                }
            };

            crawler.OnCompleted += (s, e) =>
            {
                using (var stream = new FileStream(filePathLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        var links = Regex.Match(e.PageSource, "cartoon_online_border[^$]{0,}其它汉化版");
                        var urls = Regex.Matches(links.Value,
                            @"<a[^>]+title=""*(?<title>[^>\s]+)""[^>]+href=""*(?<href>[^>\s]+)""\s*[^>]*>[^<]*</a>");

                        var cartoonName = urls[0].Groups["title"].Value.Split('-')[0];
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

                        writer.WriteLine($"{cartoonName}章节地址爬取完成！开始爬取图片地址！");
                        writer.WriteLine("耗时：" + e.Milliseconds + "毫秒");
                        writer.WriteLine("线程：" + e.ThreadId + "");
                        writer.WriteLine("地址：" + e.Uri.ToString() + "");
                        writer.WriteLine("===============================================");
                    }
                }
                GetChapterId(manager);
            };
            crawler.Start(new Uri(uri), firstReferer).Wait();
            return true;
        }

        public void GetChapterId(NameToUrlManage manager)
        {
            var refererUri = firstReferer + @"hjsw/";
            var crawler = new MyCrawler();

            crawler.OnError += (s, e) =>
            {
                using (var stream = new FileStream(filePathErrorLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        writer.WriteLine(@"图片地址爬取失败；地址：" + e.Uri + "爬取报错; 错误信息：" + e.Exception.Message + "");
                        writer.WriteLine("===============================================");
                    }
                }
            };
            crawler.OnCompleted += (s, e) =>
            {
                var pictrueFilePath = "";
                var pictrueUrlManaeger = new NameToUrlManage();
                using (var stream = new FileStream(filePathLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        var links = Regex.Match(e.PageSource, @"<script type=""text/javascript"">(?<script>[^$]*?)</script>");

                        using (var engine = new V8ScriptEngine("debug-v8engine"))
                        {
                            var script = links.Groups["script"].Value;
                            script += "arr_pages = arr_pages.join(',')";
                            engine.Execute(script);

                            //获取图片url
                            var values = (string)engine.Script.arr_pages;
                            var pictrueUrls = values.Split(',');

                            //获取章节名
                            var comicName = (string)engine.Script.g_comic_name;
                            var chapterName = (string)engine.Script.g_chapter_name;

                            //创建相应文件夹
                            pictrueFilePath = filePath + "\\" + comicName + "\\" + chapterName;
                            if (!Directory.Exists(pictrueFilePath))
                            {
                                Directory.CreateDirectory(pictrueFilePath);
                            }

                            var queue = new Queue<NameToUrl>();
                            foreach (var pictrueUrl in pictrueUrls)
                            {
                                queue.Enqueue(new NameToUrl
                                {
                                    Name = chapterName + "-" + chapterName,
                                    Url = pictureMain + pictrueUrl
                                });
                            }
                            pictrueUrlManaeger = new NameToUrlManage(queue);

                            writer.WriteLine($"{comicName + " - " + chapterName}爬取完成完成！开始爬取图片！");
                            writer.WriteLine("耗时：" + e.Milliseconds + "毫秒");
                            writer.WriteLine("线程：" + e.ThreadId + "");
                            writer.WriteLine("地址：" + e.Uri.ToString() + "");
                            writer.WriteLine("===============================================");
                        }
                    }
                }
                GetPictures(pictrueUrlManaeger, e.Uri.ToString() + "", pictrueFilePath);
                Console.WriteLine("目录：" + pictrueFilePath + "完成");
            };

            Parallel.For(0, manager.Count, i =>
            {
                var nameToUrl = manager.GetUrl();
                var url = firstReferer + nameToUrl.Url;
                var specialArguments = new Dictionary<string, string>
                {
                    { "Name" , $"{nameToUrl.Name}"},
                };
                crawler.Start(new Uri(url), refererUri, specialArguments).Wait();
            });

        }

        public void GetPictures(NameToUrlManage pictrueUrlManager, string refererUri, string pictrueFilePath)
        {
            var crawler = new MyCrawler();

            crawler.OnError += (s, e) =>
            {
                using (var stream = new FileStream(filePathErrorLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        writer.WriteLine(@"某图片爬取失败；地址：" + e.Uri + "爬取报错; 错误信息：" + e.Exception.Message + "");
                        writer.WriteLine("===============================================");
                    }
                }

            };
            crawler.OnCompleted += (s, e) =>
            {
                using (var stream = new FileStream(filePathLog, FileMode.Append, FileAccess.Write,
                    FileShare.Write))
                {
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        var uriStr = e.Uri.ToString().Split('/');
                        var pictrueFilePathCurr = pictrueFilePath + "\\" + uriStr[uriStr.Length - 1];
                        var memoryStream = new MemoryStream(e.Bitmap);
                        var bitmap = new Bitmap(memoryStream);
                        bitmap.Save(pictrueFilePathCurr, ImageFormat.Jpeg);
                        writer.WriteLine("爬虫抓取任务结束！");
                        writer.WriteLine("耗时：" + e.Milliseconds + "毫秒");
                        writer.WriteLine("线程：" + e.ThreadId + "");
                        writer.WriteLine("地址：" + e.Uri.ToString() + "");
                        writer.WriteLine("===============================================");
                    }
                }
            };

            for (int i = 0; i < pictrueUrlManager.Count; i++)
            {
                crawler.Start(new Uri(pictrueUrlManager.GetUrl().Url), refererUri).Wait();
            }
        }

    }
}