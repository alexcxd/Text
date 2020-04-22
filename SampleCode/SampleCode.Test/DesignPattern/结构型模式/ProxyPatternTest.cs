using NUnit.Framework;
using SampleCode.DesignPattern.结构型模式;

namespace SampleCode.Test.DesignPattern.结构型模式
{
    /// <summary>
    /// 代理模式
    /// </summary>
    [TestFixture]
    public class ProxyPatternTest
    {
        [Test]
        public void ProxyPatternCodeTest()
        {
            //静态代理模式
            Proxy proxy = new Proxy(new RealSubject());
            proxy.Request();

            //动态代理 继承RealProxy(.net自带)
        }
    }
}