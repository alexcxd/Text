using NUnit.Framework;
using NUnit.Framework.Internal;
using SampleCode.DesignPattern.结构型模式;

namespace SampleCode.Test.DesignPattern.结构型模式
{
    [TestFixture]
    public class AdapterPatternTest
    {
        /// <summary>
        /// 适配器模式
        /// </summary>
        [Test]
        public void AdapterPatternCodeTest()
        {
            Target adapter = new Adapter();
            adapter.Request();
        }
    }
}