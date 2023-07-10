using System;
using NUnit.Framework;
using SampleCode.DesignPattern.StructuralPatterns;

namespace SampleCode.Test.DesignPattern.StructuralPatterns
{
    [TestFixture]
    public class DecoratorPatternTest
    {
        /// <summary>
        /// 装饰器模式1
        /// </summary>
        [Test]
        public void DecoratorPatternCodeTest()
        {
            // 在原来的基础上增加功能
            Console.WriteLine("在原来的基础上增加功能");
            ConcreteComponent concreteComponent = new ConcreteComponent();

            DecoratorA decoratorA = new DecoratorA(concreteComponent);
            DecoratorB decoratorB = new DecoratorB(decoratorA);

            decoratorB.Operation();

            Console.WriteLine("-------------------------------------");

            // 覆盖掉原来的功能
            Console.WriteLine("覆盖掉原来的功能");
            CurrComponent currComponent = new CurrComponent();
            currComponent.Operation();
        }
    }
}