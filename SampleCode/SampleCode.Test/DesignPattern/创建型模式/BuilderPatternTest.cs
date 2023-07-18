using NUnit.Framework;
using SampleCode.DesignPattern.CreationalPatterns;

namespace SampleCode.Test.DesignPattern.CreationalPatterns
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
            BuilderPattern.ConcreteBuilderA concreteBuilderA = new BuilderPattern.ConcreteBuilderA();
            BuilderPattern.ConcreteBuilderB concreteBuilderB = new BuilderPattern.ConcreteBuilderB();

            BuilderPattern.Director director = new BuilderPattern.Director();
            director.GetProduct(concreteBuilderA);
            var aProduct = concreteBuilderA.GetResult();
            aProduct.Show();

            director.GetProduct(concreteBuilderB);
            var bProduct = concreteBuilderB.GetResult();
            bProduct.Show();
        }

    }
}
