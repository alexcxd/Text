using System;
using NUnit.Framework;
using SampleCode.DesignPattern.结构型模式;

namespace SampleCode.Test.DesignPattern.结构型模式
{
    [TestFixture]
    public class CompositePatternTest
    {
        [Test]
        public void CompositePatternCodeTest()
        {
            CompositePattern.CompositePattern1();

            Console.WriteLine("-------------");

            CompositePattern.CompositePattern2();
        }
    }
}