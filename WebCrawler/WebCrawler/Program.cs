using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using WebCrawler.Bussiness;


namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var dmzj = new DmzjCrawlerBussiness();
            dmzj.GetAllChapters();

            //ClearScriptTest.CleaScriptTestMain();

            Console.ReadKey();
        }

        public static void Test()
        {
            string url = "https://images.dmzj.com/h/%E9%BB%84%E9%87%91%E7%A5%9E%E5%A8%81/%E7%AC%AC161%E8%AF%9D/02.jpg";
            string url1 = "https://images.dmzj.com/h/%E9%BB%84%E9%87%91%E7%A5%9E%E5%A8%81/%E7%AC%AC01%E8%AF%9D/35.jpg";
            //string url = "https://manhua.dmzj.com/hjsw/78305.shtml";
            string oriUrl = "https://manhua.dmzj.com/hjsw/78305.shtml";
            string filePath = @"E:\Desktop\漫画\2.jpg";
            string filePath1 = @"E:\Desktop\漫画\1.jpg";
            string userAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36";
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url1);
            hwr.Referer = oriUrl;
            hwr.UserAgent = userAgent;

            //FileStream fs = new FileStream(filePath, FileMode.Create);

            WebResponse hwResponse = hwr.GetResponse();

            using (var streamIn = hwResponse.GetResponseStream())
            {
                using (var ms = new MemoryStream())
                {
                    //多线程要注意流被关闭的问题
                    streamIn.CopyTo(ms);
                    Bitmap bitmap = new Bitmap(ms);
                    bitmap.Save(filePath1, ImageFormat.Jpeg);
                }
            }
        }
    }
}
