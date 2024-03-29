﻿using NUnit.Framework;
using NUnit.Framework.Internal;
using SampleCode.DesignPattern.StructuralPatterns;

namespace SampleCode.Test.DesignPattern.StructuralPatterns
{
    [TestFixture]
    public class BridgePatternTest
    {
        /// <summary>
        /// 桥接模式
        /// </summary>
        [Test]
        public void BridgePatternCodeTest()
        {
            RefinedAbstranction abstranction = new RefinedAbstranction();
            abstranction.SetImplementor(new ConcreteImplementorA());
            abstranction.Operation();

            abstranction.SetImplementor(new ConcreteImplementorB());
            abstranction.Operation();
        }

    }
}