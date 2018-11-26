using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using WebCrawler.Model;

namespace WebCrawler.Bussiness
{
    /// <summary>
    /// 爬取动漫之家
    /// </summary>
    public class DmzjCrawlerBussiness
    {
        private static string firstReferer = "https://manhua.dmzj.com/";
        private static string filePath = @"E:\Desktop\漫画";
        private static string filePathLog = filePath + @"\Log.txt";

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
                        writer.Write(@"地址：" + e.Uri + "开始爬取");
                    };

                    crawler.OnError += (s, e) =>
                    {
                        writer.Write(@"地址：" + e.Uri + "爬取报错; 错误信息：" + e.Exception.Message);
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
                        var manager = new NameToUrlManage(urlQueue);
                    };
                    crawler.Start(new Uri(uri), firstReferer).Wait();
                }
            }
            return true;
        }
    }
}