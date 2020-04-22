using NUnit.Framework;
using SampleCode.DesignPattern.行为型模式;

namespace SampleCode.Test.DesignPattern.行为型模式
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
            Subject1 subject = new ConcreteSubserver();
            subject.Attach(new ConcreteObserver("订阅者A"));
            subject.Attach(new ConcreteObserver("订阅者B"));
            subject.Attach(new ConcreteObserver("订阅者C"));

            subject.info = "某种变化";
            subject.Notify();
        }
    }
}