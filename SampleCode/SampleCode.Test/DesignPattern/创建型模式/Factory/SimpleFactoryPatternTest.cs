using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleCode.Test.DesignPattern.Factory
{
    public class SimpleFactoryPatternTest
    {
        [Test]
        public void SimpleFactoryPatternCodeTest()
        {
            var oper = OperationFactory.CreateOperation("-");
            oper.NumbleA = 1;
            oper.NumbleB = 3;
            Console.WriteLine("结果为：" + oper.GetResult());
            Console.ReadKey();
        }
    }
}
