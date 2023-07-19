using NUnit.Framework;
using SampleCode.DesignPattern.BehavioralPatterns;

namespace SampleCode.Test.DesignPattern.BehavioralPatterns
{
    [TestFixture]
    public class ObserverPatternTest
    {
        /// <summary>
        /// 观察者模式
        /// </summary>
        [Test]
        public void ObserverPatternCodeTest()
        {
            Subject subject = new ConcreteSubserver();
            subject.Attach(new ConcreteObserver("订阅者A"));
            subject.Attach(new ConcreteObserver("订阅者B"));
            subject.Attach(new ConcreteObserver("订阅者C"));

            subject.info = "某种变化";
            subject.Notify();
        }
    }
}