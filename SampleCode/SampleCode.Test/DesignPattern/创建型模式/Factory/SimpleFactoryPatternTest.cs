﻿using System;
using NUnit.Framework;
using SampleCode.DesignPattern.创建型模式.Factory;

namespace SampleCode.Test.DesignPattern.创建型模式.Factory
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
