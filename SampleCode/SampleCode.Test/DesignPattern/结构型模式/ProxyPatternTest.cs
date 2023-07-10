using NUnit.Framework;
using SampleCode.DesignPattern.StructuralPatterns;

namespace SampleCode.Test.DesignPattern.StructuralPatterns
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