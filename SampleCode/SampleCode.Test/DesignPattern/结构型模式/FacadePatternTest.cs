using NUnit.Framework;
using SampleCode.DesignPattern.结构型模式;

namespace SampleCode.Test.DesignPattern.结构型模式
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