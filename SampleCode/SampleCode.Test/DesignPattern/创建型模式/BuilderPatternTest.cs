using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Test.DesignPattern;

namespace SampleCode.Test.DesignPattern
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
