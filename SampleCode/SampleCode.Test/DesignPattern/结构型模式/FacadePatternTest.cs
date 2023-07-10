using NUnit.Framework;
using SampleCode.DesignPattern.StructuralPatterns;

namespace SampleCode.Test.DesignPattern.StructuralPatterns
{
    [TestFixture]
    public class FacadePatternTest
    {
        /// <summary>
        /// 外观模式
        /// </summary>
        [Test]
        public void FacadePatternCodeTest()
        {
            Facade facade = new Facade();
            facade.MethodA();
            facade.MethodB();
        }
    }
}