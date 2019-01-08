using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using WebCrawler.Crawler;
using WebCrawler.Model;

namespace WebCrawler.Bussiness
{
    public class TelephoneNumberHomeBaiduBussiness
    {
        /// <summary>
        /// 通过百度的接口，模糊查询手机归属地
        /// </summary>
        public void GetHome()
        {
            string urlBase = "https://sp0.baidu.com/8aQDcjqpAAV3otqbppnN2DJv/api.php?query=166{0}32&co=&resource_id=6004&t=1546917629538&ie=utf8&oe=gbk&cb=op_aladdin_callback&format=json&tn=baidu&cb=jQuery110208575602481337634_1546917475552&_=1546917475574";
            string urlReferer = "https://www.baidu.com/s?wd=18158510455&rsv_spt=1&rsv_iqid=0x9af9acf600007f1a&issp=1&f=8&rsv_bp=0&rsv_idx=2&ie=utf-8&tn=baiduhome_pg&rsv_enter=1&rsv_sug3=11&rsv_sug1=6&rsv_sug7=100&rsv_sug2=0&inputT=3233&rsv_sug4=3234";
            var crawler = new MyCrawler();
            for (int i = 380000; i < 1000000; i+=1000)
            {
                var numberFull = new StringBuilder(i.ToString());
                if (numberFull.Length != 6)
                {
                    var length = numberFull.Length;
                    for (int j = 0; j < 6 - length; j++)
                    {
                        numberFull.Insert(0, $"0");
                    }
                }
                var url = String.Format(urlBase, numberFull);
                crawler.OnCompleted += (s, e) =>
                {
                    var data = Regex.Matches(e.PageSource,
                        "\"city\":\"(?<city>[^\"]+)\"[^}]*\"origphoneno\":\"(?<origphoneno>[^\"]+)\"");
                    var city = data[0].Groups["city"].Value;
                    var origphoneno = data[0].Groups["origphoneno"].Value;
                    if (city.Equals("郑州") || city.Equals("焦作"))
                    {
                        Console.WriteLine($"城市:{city},手机:{origphoneno}");
                    }
                };
                crawler.Start(new Uri(url), urlReferer).Wait();
            }

        }
    }
}