using NUnit.Framework;
using SampleCode.DesignPattern.创建型模式;

namespace SampleCode.Test.DesignPattern.创建型模式
{
    [TestFixture]
    public class BuilderPatternTest
    {
        /// <summary>
        /// 建造者模式
        /// </summary>
        [Test]
        public void BuilderPatternCodeTest()
        {
            ConcreteBuilderA concreteBuilderA = new ConcreteBuilderA();
            ConcreteBuilderB concreteBuilderB = new ConcreteBuilderB();

            Director director = new Director();
            director.Construct(concreteBuilderA);
            var aProcudt = concreteBuilderA.GetResult();
            aProcudt.Show();

            director.Construct(concreteBuilderB);
            var bProduct = concreteBuilderB.GetResult();
            bProduct.Show();
        }

    }
}
