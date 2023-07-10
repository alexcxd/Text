using NUnit.Framework;
using SampleCode.DesignPattern.BehavioralPatterns;

namespace SampleCode.Test.DesignPattern.BehavioralPatterns
{
    [TestFixture]
    public class StrategyPatternTest
    {
        /// <summary>
        /// 策略模式
        /// </summary>
        [Test]
        public void StrategyPatternCodeTest()
        {
            Context context;

            context = new Context(new ConcreteStrategyA());
            context.ContextInterface();

            context = new Context(new ConcreteStrategyB());
            context.ContextInterface();
        }
    }
}