using NUnit.Framework;
using SampleCode.DesignPattern.行为型模式;

namespace SampleCode.Test.DesignPattern.行为型模式
{
    [TestFixture]
    public class ChainOfResponsibilityPatternTest
    {
        /// <summary>
        /// 责任链模式
        /// </summary>
        [Test]
        public void ChainOfResponsibilityPatternCodeTest()
        {
            ConcreteHandlerA handlerA = new ConcreteHandlerA();
            ConcreteHandlerB handlerB = new ConcreteHandlerB();

            handlerA.SetSuccessor(handlerB);

            handlerA.HandleRequest(1);
            handlerA.HandleRequest(11);
        }
    }
}