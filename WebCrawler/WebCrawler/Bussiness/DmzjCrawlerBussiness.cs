using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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
        private static NameToUrlManage manager = new NameToUrlManage();

        public bool GetAllChapters()
        {
            var chapterList = new List<NameToUrl>();
            var uri = firstReferer + @"hjsw/";
            var crawler = new MyCrawler();
            using (var stream = new FileStream(filePathLog, FileMode.Append))
            {
                using (var writer = new StreamWriter(stream))
                {
                    crawler.OnStart += (s, e) =>
                    {
                        Console.WriteLine("==================任务一开始=======================");
                        writer.Write(@"地址：" + e.Uri + "开始爬取");
                    };

                    crawler.OnError += (s, e) =>
                    {
                        writer.Write(@"地址：" + e.Uri + "爬取报错; 错误信息：" + e.Exception.Message);
                        Console.WriteLine("===============================================");
                    };

                    crawler.OnCompleted += (s, e) =>
                    {

                        var links = Regex.Match(e.PageSource, "cartoon_online_border[^$]{0,}其它汉化版");
                        var urls = Regex.Matches(links.Value, @"<a[^>]+title=""*(?<title>[^>\s]+)""[^>]+href=""*(?<href>[^>\s]+)""\s*[^>]*>[^<]*</a>");

                        var urlQueue = new Queue<NameToUrl>();
                        foreach (Match url in urls)
                        {
                            var nameToUrl = new NameToUrl();
                            nameToUrl.Name = url.Groups["title"].Value;
                            nameToUrl.Url = url.Groups["href"].Value;
                            urlQueue.Enqueue(nameToUrl);
                        }
                        manager = new NameToUrlManage(urlQueue);

                        writer.Write("爬虫抓取任务完成！开始步骤二！");
                        writer.Write("耗时：" + e.Milliseconds + "毫秒");
                        writer.Write("线程：" + e.ThreadId);
                        writer.Write("地址：" + e.Uri.ToString());
                        writer.Write("===============================================");
                        GetChapterId();

                    };
                }
            }
            crawler.Start(new Uri(uri), firstReferer).Wait();

            return true;
        }

        public void GetChapterId()
        {
            var refererUri = firstReferer + @"hjsw/";
            var crawler = new MyCrawler();

            using (var stream = new FileStream(filePathLog, FileMode.Append))
            {
                using (var writer = new StreamWriter(stream))
                {
                    crawler.OnStart += (s, e) =>
                    {

                        writer.Write("===================任务二开始======================");
                        writer.Write(@"地址：" + e.Uri + "开始爬取");

                    };

                    crawler.OnError += (s, e) =>
                    {

                        writer.Write(@"地址：" + e.Uri + "爬取报错; 错误信息：" + e.Exception.Message);
                        writer.Write("===============================================");

                    };
                    crawler.OnCompleted += (s, e) =>
                    {

                        var links = Regex.Match(e.PageSource, @"^\|B1_[^|]*\|$");
                        var id = links.Value.Substring(3, links.Value.Length - 1);
                        var name = e.SpecialArguments["Name"].Split('-');
                        var url = UrlUtil.UrlEncode(name[0]) + "/" + UrlUtil.UrlEncode(name[1]) + id;

                        writer.Write("爬虫抓取任务完成！开始步骤三！");
                        writer.Write("耗时：" + e.Milliseconds + "毫秒");
                        writer.Write("线程：" + e.ThreadId);
                        writer.Write("地址：" + e.Uri.ToString());
                        writer.Write("===============================================");

                    };
                }
            }
            Parallel.For(0, manager.Count, i =>
            {
                var nameToUrl = manager.GetUrl();
                var url = firstReferer + nameToUrl.Url;
                var specialArguments = new Dictionary<string, string>
                {
                    { "Name" , $"{nameToUrl.Name}"},
                };
                crawler.Start(new Uri(url), refererUri, specialArguments);
            });

        }
    }
}