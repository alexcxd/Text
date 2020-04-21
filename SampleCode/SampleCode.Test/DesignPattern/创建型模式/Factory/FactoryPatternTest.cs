using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Test.DesignPattern;

namespace SampleCode.Test.DesignPattern.Factory
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
