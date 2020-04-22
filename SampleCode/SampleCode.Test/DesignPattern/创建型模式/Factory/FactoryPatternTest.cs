using System;
using NUnit.Framework;
using SampleCode.DesignPattern.创建型模式.Factory;

namespace SampleCode.Test.DesignPattern.创建型模式.Factory
{
    public class FactoryPatternTest
    {
        /// <summary>
        /// 工厂模式
        /// </summary>
        [Test]
        public void FactoryPatternCodeTest()
        {
            var factory = new AddFactory();
            var opereation = factory.CreateOpereation();
            opereation.NumbleA = 100;
            opereation.NumbleB = 10.1;
            var result = opereation.GetResult();
            Console.WriteLine(result);
        }
    }
}
