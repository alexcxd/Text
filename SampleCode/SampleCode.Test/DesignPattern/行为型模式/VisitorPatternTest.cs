using NUnit.Framework;
using NUnit.Framework.Internal;
using SampleCode.DesignPattern.BehavioralPatterns;

namespace SampleCode.Test.DesignPattern.BehavioralPatterns
{
    /// <summary>
    /// 访问者模式
    /// </summary>
    [TestFixture]
    public class VisitorPatternTest
    {
        [Test]
        public void VisitorPatternCodeTest()
        {
            ConcreteElementA elementA = new ConcreteElementA();
            ConcreteElementB elementB = new ConcreteElementB();
            ObjectStruture struture = new ObjectStruture();
            struture.Add(elementA);
            struture.Add(elementB);

            ConcreteVisitor1 v1 = new ConcreteVisitor1();
            struture.Aceppt(v1);
        }
    }
}