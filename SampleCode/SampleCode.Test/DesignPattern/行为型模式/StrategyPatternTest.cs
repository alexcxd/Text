using NUnit.Framework;
using SampleCode.DesignPattern.行为型模式;

namespace SampleCode.Test.DesignPattern.行为型模式
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