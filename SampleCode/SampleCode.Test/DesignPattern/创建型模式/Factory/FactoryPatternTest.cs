using System;
using NUnit.Framework;
using SampleCode.DesignPattern.CreationalPatterns.Factory;

namespace SampleCode.Test.DesignPattern.CreationalPatterns.Factory
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
            var operation = factory.CreateOperation();
            operation.NumberA = 100;
            operation.NumberB = 10.1;
            var result = operation.GetResult();
            Console.WriteLine(result);
        }
    }
}
