using System;
using System.Net;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.Network
{
    [TestFixture]
    public class UriTest
    {
        /// <summary>
        /// URI基本操作
        /// </summary>
        [Test]
        public void UriCodeTest()
        {
            var a = ServicePointManager.DefaultConnectionLimit;

            //URI是一个具有特殊类型的字符串, 它描述了Internet或LAN资源
            //URI由3部分组成(协议、权限和路径), 例如http://baidu.com/index.html,
            //http为协议, baidu.com为权限, index.html为路径

            //Uri类适用于验证URI字符串的格式,
            //Uri的构造器在创建Uri类型时, 文件和UNC路径自动转换URI(添加协议file:, 并将反斜杠转换为斜杠), 
            //将协议和主机名转换为小写, 删除默认端口或空端口
            //网页地址
            var uriWeb = new Uri("https://www.baidu.com/index.html?Id=10");
            var isLoopback1 = uriWeb.IsLoopback;    //IsLoopback(127.0.0.1)属性表示Uri是否引用本地主机
            var scheme = uriWeb.Scheme;                 //协议
            var host = uriWeb.Host;                     //地址
            var port = uriWeb.Port;                     //端口
            var pathAndQuery = uriWeb.PathAndQuery;     //路径和参数

            //主机地址
            var uriLocal = new Uri("http://127.0.0.1");
            var isLoopback2 = uriLocal.IsLoopback;    //true

            //文件和UNC路径
            var uriFile = new Uri(@"D:\Desktop\3.jpg");
            var isFile = uriFile.IsFile;    //IsFile表示是否引用一个本地或者UNC路径
            var localPath = uriFile.LocalPath;  //返回一个符合本地操作系统命名习惯的绝对路径

            //Uri中有部分只读属性, 若希望修改可以通过创建一个UriBulider对象实现
            var uriBefore = new Uri("https://www.baidu.com/index.html?Id=10");
            var uriBuilder = new UriBuilder(uriBefore) { Host = "www.qq.com" };
            var uriAftter = uriBuilder.Uri;

            //Uri类还提供了一些静态的辅助方法
            //EscapeUriString()方法将ASCII值大于127的所有字符转换为十六进制
            var escapeUriString = Uri.EscapeUriString("https://www.baidu.com/index.html?text=你好");

            //URI后的斜杠是很重要的, 服务器会根据它来决定该URI是否包含路径
            //例如, 假设URI为http://www.albahari.com/nutshell/那么HTTP服务器就会在网站的Web文件夹中查找名为nutshell的子文夹,
            //并返回内部的默认文档(index.html)。若URI结尾处没有斜杠, 则WEP服务器则会试图在网站的根目录下寻找名为nutshell的文件。
        }
    }
}
