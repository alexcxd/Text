using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SampleCode.Test.Network
{
    /// <summary>
    /// HttpCliet
    /// </summary>
    [TestFixture]
    public class HttpClientTest
    {
        #region HttpClient基本操作

        /// <summary>
        /// HttpClient基本操作
        /// </summary>
        [Test]
        public async Task HttpClientCodeTest()
        {
            //HttpClient
            //优点: 1.一个HttpClient可以就可以支持并发请求
            //      2.HttpClient支持插件式的自定义消息处理器
            //      3.HttpClient有丰富的易于扩展的请求头部与内容类型
            //缺点: 1.不支持进度报告, 且仅支持Http协议

            //最简单的使用方式, 直接实例化并调用GetXXXAsync方法
            var html = await new HttpClient().GetStringAsync("https://www.baidu.com");

            //HttpClient包含Timeout和BaseAddress属性, 其中大多数属性都定义在HttpClientHandler类中
            var handler = new HttpClientHandler
            {
                Proxy = null,                       //代理
                MaxConnectionsPerServer = 4,        //最大Http并行数
                Credentials = new CredentialCache() //身份认证
            };
            var clientTohandler = new HttpClient(handler)
            {
                Timeout = new TimeSpan(1000)    //超时时间

            };

            //GetAsync方法(GetXXXAsync方法是它的快捷调用)返回一个响应消息(HttpResponseMessage)
            //PostAsync, PutAsync, DeleteAsync和GetAsync类似
            var clientToHttpVerbs = new HttpClient();
            var responseToHttpVerbs = await clientToHttpVerbs.GetAsync("https://www.baidu.com");
            responseToHttpVerbs.EnsureSuccessStatusCode(); //除非显示调用该方法, 否则失败时只会返回错误码(StatusCode), 而不是抛出异常
            var resultToHttpVerbs = await responseToHttpVerbs.Content.ReadAsStringAsync();    //也可以通过Content类的CopyToAsync()方法将内容写入到另一个流

            //SendAsync方法(GetAsync, PostAsync, PutAsync, DeleteAsync是它的快捷调用)
            //SendAsync是最底层的方法(最灵活)
            //使用SendAsync方法需要创建HttpRequestMessage方法
            var clientToSend = new HttpClient();
            var requestToSend = new HttpRequestMessage(HttpMethod.Get, "https://www.baidu.com");
            var responseToSend = await clientToSend.SendAsync(requestToSend);
            responseToSend.EnsureSuccessStatusCode(); //除非显示调用该方法, 否则失败时只会返回错误码(StatusCode), 而不是抛出异常
            var resultToSend = await responseToSend.Content.ReadAsStringAsync();
        }

        #endregion

        #region DelegatingHandler

        /// <summary>
        /// 通过DelegatingHandler实现自定义身份验证、压缩以及加密等功能
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task DelegatingHandlerTest()
        {
            //DelegatingHandler是HttpMessageHandler的子类
            //可以实现HttpClient自定义身份验证、压缩以及加密等功能
            //以下实现一个简单的日志功能
            var clientToLog = new HttpClient(new Logginghandler(new HttpClientHandler()));
            var requestToLog = new HttpRequestMessage(HttpMethod.Get, "https://www.baidu.com");
            var responseToLog = await clientToLog.SendAsync(requestToLog);
        }

        public class Logginghandler : DelegatingHandler
        {
            public Logginghandler(HttpMessageHandler nextHandler)
            {
                InnerHandler = nextHandler;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Console.WriteLine("Requesting:" + request.RequestUri);
                var response = await base.SendAsync(request, cancellationToken);
                Console.WriteLine("Got Response:" + response.StatusCode);

                return response;
            }
        }

        #endregion

        #region 代理

        /// <summary>
        /// 代理
        /// </summary>
        [Test]
        public async Task HttpClientProxyTest()
        {
            var p = new WebProxy("187.130.75.77:3128");
            var handler = new HttpClientHandler()
            {
                Proxy = p,
                //UseProxy = true
            };
            var client = new HttpClient(handler);

            var result = await client.GetStringAsync("https://www.dmzj.com/");

            Console.Write(result);
        }

        #endregion

        #region 身份验证

        /// <summary>
        /// 身份验证
        /// </summary>
        [Test]
        public void HttpClientCredentialTest()
        {
            //方法一
            //通过HttpClientHandler的Credentials属性
            //NetworkCredential适用于基于对话框的身份验证(Digest和Basic)
            //CredentialCache可以强制使用特定的身份验证协议
            var cache = new CredentialCache();
            var prefix = new Uri("http://www.baidu.com");
            cache.Add(prefix, "Digest", new NetworkCredential("username", "password"));
            cache.Add(prefix, "Negotitate", new NetworkCredential("username", "password"));
            var handler = new HttpClientHandler()
            {
                //Credentials = new NetworkCredential("username", "password"),
                Credentials = cache
            };
            var client1 = new HttpClient(handler);

            //方法二
            //使用头部信息
            var client2 = new HttpClient();
            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes("username:password")));
        }

        #endregion

        #region 异常处理

        /// <summary>
        /// 异常处理
        /// </summary>
        [Test]
        public async Task HttpClientExceptionTest()
        {
            //HttpClient将WebException包装为HtppRequestException
            //只有在响应对象上调用EnsureSuccessStatusCode时才会抛出异常
            //可以通过响应对象的StatusCode属性获得请求状态码
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("http://1.dkytest.cn/pc/auth/getPcMallName");
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region 头部信息

        /// <summary>
        /// 头部信息
        /// </summary>
        [Test]
        public async Task HttpClientHeaderTest()
        {
            //方法一 通过HttpClient的DefaultRequestHeaders属性
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("VisualStudio", "2019"));

            //方法二 通过HttpRequestMessage的Header属性
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.baidu.com");
            request.Headers.Add("VisualStudio", "2019");
            var response = await client.SendAsync(request);
        }

        #endregion

        #region 查询字符串

        /// <summary>
        /// 查询字符串
        /// </summary>
        [Test]
        public void HttpClientQueryStrTest()
        {
            //查询字符串是一个以问号开始的, 附加在URI后的字符串, 用于向服务器发送简单的数据
            //建议使用Uri.EscapeDataString进行字符转义(会转换特殊字符)
            var search = Uri.EscapeDataString("（WebClient Or HttpClient）");
            var requestUri = $"https://www.baidu.com?q={search}";
        }

        #endregion

        #region 上传表单数据

        /// <summary>
        /// 上传表单数据
        /// </summary>
        [Test]
        public async Task HttpClientFormDataTest()
        {
            //HttpClient需要创建一个FromUrlEncodeContent对象, 并将其传个PostAnsy,
            //或者赋给请求的Content参数

            var uri = $"http://www.albahari.com/EchoPost.aspx";
            var client = new HttpClient();
            var dict = new Dictionary<string, string>
            {
                { "Name", "Joe Albahari" },
                { "Company", "O' Reilly" }
            };
            var values = new FormUrlEncodedContent(dict);

            //方式一
            var response1 = await client.PostAsync(uri, values);
            response1.EnsureSuccessStatusCode();
            Console.WriteLine(await response1.Content.ReadAsStringAsync());

            //方式二
            var request = new HttpRequestMessage(HttpMethod.Post, uri) { Content = values };
            var response2 = await client.SendAsync(request);
            response2.EnsureSuccessStatusCode();
            Console.WriteLine(await response2.Content.ReadAsStringAsync());
        }

        #endregion

        #region cookie

        /// <summary>
        /// cookie
        /// </summary>
        [Test]
        public void HttpClientCookieTest()
        {
            //cookie是一种名称/值的字符串对, Http服务器将其放在响应的头部信息中发给客户端
            //默认情况下会忽略从服务器接受cookie, 若需要接受cookie需要创建一个CookieContainer对象
            //如果要复用cookie(多个请求使用一个cookie), 可以使用同一个HttpClient进行请求
            var cookie = new CookieContainer();
            var handler= new HttpClientHandler
            {
                Proxy = null,
                CookieContainer = cookie
            };
            var client = new HttpClient(handler);
        }

        #endregion
    }
}