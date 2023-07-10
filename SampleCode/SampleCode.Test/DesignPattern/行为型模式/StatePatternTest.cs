using NUnit.Framework;
using SampleCode.DesignPattern.BehavioralPatterns;

namespace SampleCode.Test.DesignPattern.BehavioralPatterns
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