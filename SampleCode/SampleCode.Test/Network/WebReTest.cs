using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SampleCode.Test.Network
{
    /// <summary>
    /// WebRequest和WebResponse
    /// </summary>
    [TestFixture]
    public class WebReTest
    {
        #region WebRequest和WebResponse基本操作

        /// <summary>
        /// WebRequest和WebResponse基本操作
        /// </summary>
        [Test]
        public async Task WebReCodeTest()
        {
            //WebRequest和WebResponse是管理http和ftp客户端活动, 以及"file:"协议的通用基类
            //一个WebRequest对象不可用于多个请求, 每一个实例仅可用于一个作业

            //WebRequest和WebResponse使用步骤:
            //1.使用Uri调用WebRequest.Create来实例一个Web请求对象
            //2.设置Proxy值
            //3.若需要验证,则设置Credentials属性
            //4.如果上传数据则调用请求对象的GetRequestStream方法, 并向流中写入数据
            //5.如果需要下载数据调用请求对象的GetResponse方法, 实例一个Web响应对象
            //6.在Web响应对象上调用GetResponseStream方法, 并从流中读取数据

            //下载指定文件
            //同步实现
            var request1 = WebRequest.Create("http://www.baidu.com");   //Create静态方法会创建一个WebRequest实例, 并根据URI前缀来选择子类
            request1.Timeout = 1000;  //Timeout用于设置超时时间(ms), 若超时会抛出异常。Http协议默认超时时间为100秒, Ftp默认不限制
            request1.Proxy = null;
            request1.Credentials = new CredentialCache();
            request1.Method = "GET";
            using var response1 = request1.GetResponse();
            using var stream1 = response1.GetResponseStream();
            using var fs1 = File.Create(@"D:\Desktop\code.html");
            stream1.CopyTo(fs1);
            //异步实现
            var request2 = WebRequest.Create("http://www.baidu.com");
            request2.Proxy = null;
            request2.Credentials = new CredentialCache();
            request2.Method = "GET";
            using var response2 = await request2.GetResponseAsync();
            using var stream2 = response2.GetResponseStream();
            using var fs2 = File.Create(@"D:\Desktop\code.html");
            await stream2.CopyToAsync(fs2);
        }

        #endregion

        #region 代理

        /// <summary>
        /// 代理
        /// </summary>
        [Test]
        public void WebReProxyTest()
        {
            //如果不使用代理, 必须将Proxy属性置为null, 否则会自动检查代理设置（会花费大量的时间）
            var proxy = new WebProxy("115.221.244.13:8885");
            var request = WebRequest.Create("http://www.baidu.com");
            request.Method = "GET";
            request.Proxy = proxy;
            using var response = (HttpWebResponse)request.GetResponse();
            using var stream = response.GetResponseStream();
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;
            var bytes = new byte[ms.Length];
            ms.Read(bytes, 0, bytes.Length);
            var str = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(str);

            //设置全局代理
            WebRequest.DefaultWebProxy = proxy;
        }

        #endregion

        #region 上传表单数据

        /// <summary>
        /// 上传表单数据
        /// </summary>
        [Test]
        public void WebReFormDataTest()
        {
            //1.将请求的ContentType设置为"application/x-www-form-urlencoded", Methon设置未Post
            //2.构建包含上传数据的字符串, 并将其编译为:name1=value1&name2=value2
            //3.使用Encoding.UTF8.GetBytes方法将字符串转换为一个字节数组
            //4.将请求的ContentLength属性的值设置为字节数组的长度
            //5.调用请求对象的GetRequestStream方法, 将字节数组写入到流
            //6.调用GetResponse来读取服务器的响应

            var request = WebRequest.Create("http://www.albahari.com/EchoPost.aspx");
            request.Proxy = null;
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";

            var reqString = "Name=Joe+Albahari&CompanyO'Reilly";
            var bytes = Encoding.UTF8.GetBytes(reqString);
            request.ContentLength = bytes.Length;

            using (var reqStream = request.GetRequestStream())
            {
                reqStream.Write(bytes);
            }

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var sr = new StreamReader(stream))
            {
                Console.WriteLine(sr.ReadToEnd());
            }
        }

        #endregion

        #region cookie

        /// <summary>
        /// cookie
        /// </summary>
        [Test]
        public void WebReCookieTest()
        {
            //cookie是一种名称/值的字符串对, Http服务器将其放在响应的头部信息中发给客户端
            //默认情况下会忽略从服务器接受cookie, 若需要接受cookie需要创建一个CookieContainer对象
            var cookie = new CookieContainer();
            var request = (HttpWebRequest) WebRequest.Create("");
            request.CookieContainer = cookie;
        }

        #endregion
    }
}