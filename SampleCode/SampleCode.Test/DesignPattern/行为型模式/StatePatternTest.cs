using NUnit.Framework;
using SampleCode.DesignPattern.行为型模式;

namespace SampleCode.Test.DesignPattern.行为型模式
{
    [TestFixture]
    public class StatePatternTest
    {
        /// <summary>
        /// 状态模式
        /// </summary>
        [Test]
        public void StatePatternCodeTest()
        {
            StateContext context = new StateContext(new ConcreteStateA());
            context.Request();
            context.Request();
            context.Request();
            context.Request();
        }
    }
}