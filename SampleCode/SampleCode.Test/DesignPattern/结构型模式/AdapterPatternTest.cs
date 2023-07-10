using NUnit.Framework;
using NUnit.Framework.Internal;
using SampleCode.DesignPattern.StructuralPatterns;

namespace SampleCode.Test.DesignPattern.StructuralPatterns
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