using NUnit.Framework;
using SampleCode.DesignPattern.BehavioralPatterns;
using SampleCode.DesignPattern.BehavioralPatterns.ChainOfResponsibility;

namespace SampleCode.Test.DesignPattern.BehavioralPatterns
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