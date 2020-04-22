using NUnit.Framework;
using NUnit.Framework.Internal;
using SampleCode.DesignPattern.结构型模式;

namespace SampleCode.Test.DesignPattern.结构型模式
{
    [TestFixture]
    public class BridgePatternTest
    {
        /// <summary>
        /// 桥接模式
        /// </summary>
        [Test]
        public void BridgePatternCodeTest()
        {
            RefinedAbstranction abstranction = new RefinedAbstranction();
            abstranction.SetImplementor(new ConreteImplementorA());
            abstranction.Operation();

            abstranction.SetImplementor(new ConreteImplementorB());
            abstranction.Operation();
        }

    }
}