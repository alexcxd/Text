using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SampleCode.Test.Network
{
    /// <summary>
    /// WebClient
    /// </summary>
    [TestFixture]
    public class WebClientTest
    {
        #region WebClient基本操作

        /// <summary>
        /// WebClient基本操作
        /// </summary>
        [Test]
        public async Task WebClientCodeTest()
        {
            //WebClient是一个易于使用的门面类(外观模式),负责调用WebRequest和WebResponse的功能
            //WebClient实现了IDisposable, 但是由于它的Dispose方法在运行时并没有执行太多实际操作, 因此不需要销毁

            //WebClient使用步骤:
            //1.实例化一个WebClient
            //2.设置Proxy值
            //3.若需要验证,则设置Credentials属性
            //4.使用对应的URI调用DownloadXXX或者UploadXXX方法

            //下载指定文件到指定位置
            //同步实现
            var wc1 = new WebClient { Proxy = null };
            wc1.Credentials = new CredentialCache();
            wc1.DownloadFile("http://www.albahari.com/nutshell/code.aspx", @"D:\Desktop\code.htm");
            //异步实现，以及取消操作和进度报告
            //其中如果使用了取消操作或进度报告, 请避免对一个WebClient多次操作, 因为这样会形成竞争条件
            var wc2 = new WebClient { Proxy = null };
            wc2.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage + "%");  //进度报告
            Task.Delay(5000).ContinueWith(x => wc2.CancelAsync());  //5秒后取消请求, 取消时会抛出异常
            await wc2.DownloadFileTaskAsync("http://www.baidu.com", @"D:\Desktop\webpage.htm");
        }

        #endregion

        #region 代理

        /// <summary>
        /// 代理
        /// </summary>
        [Test]
        public async Task WebClientProxyTest()
        {
            //如果不使用代理, 必须将Proxy属性置为null, 否则会自动检查代理设置（会花费大量的时间）
            var proxy = new WebProxy("115.221.244.13:8885");
            var webClient = new WebClient
            {
                Proxy = proxy
            };
            var str = webClient.DownloadString("http://www.baidu.com");
            Console.WriteLine(str);

        }

        #endregion

        #region 查询字符串

        /// <summary>
        /// 查询字符串
        /// </summary>
        [Test]
        public void WebClientQueryStrTest()
        {
            //查询字符串是一个以问号开始的, 附加在URI后的字符串, 用于向服务器发送简单的数据
            var webClient = new WebClient { Proxy = null };
            webClient.QueryString.Add("q", "WebClient");
            webClient.QueryString.Add("Hl", "2020/09/10 20:00:00");
        }

        #endregion

        #region 上传表单数据

        /// <summary>
        /// 上传表单数据
        /// </summary>
        [Test]
        public void WebClientFormDataTest()
        {
            //WebClient提供了UploadValues方法用来提交表单数据
            //NameValueCollection中的键与HTML表单的输入框相对应
            var wc = new WebClient(){Proxy = null};
            var data = new NameValueCollection
            {
                {"Name", "Joe Albahari"}, 
                {"Company", "O'Reilly"}
            };
            var result = wc.UploadValues("http://www.albahari.com/EchoPost.aspx", data);
            Console.WriteLine(Encoding.UTF8.GetString(result));
        }

        #endregion

        #region cookie

        /// <summary>
        /// cookie
        /// </summary>
        [Test]
        public void WebClientCookieTest()
        {
            //WebClient不支持Cookie
        }

        #endregion
    }
}