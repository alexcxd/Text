using System;
using NUnit.Framework;
using SampleCode.DesignPattern.CreationalPatterns.Factory;

namespace SampleCode.Test.DesignPattern.CreationalPatterns.Factory
{
    public class SimpleFactoryPatternTest
    {
        [Test]
        public void SimpleFactoryPatternCodeTest()
        {
            var oper = OperationFactory.CreateOperation("-");
            oper.NumberA = 1;
            oper.NumberB = 3;
            Console.WriteLine("结果为：" + oper.GetResult());
            Console.ReadKey();
        }
    }
}
