using System;
using NUnit.Framework;
using SampleCode.DesignPattern.StructuralPatterns;

namespace SampleCode.Test.DesignPattern.StructuralPatterns
{
    [TestFixture]
    public class CompositePatternTest
    {
        [Test]
        public void CompositePatternCodeTest()
        {
            SafeCompositePattern.SafeCompositePatternMain();

            Console.WriteLine("-------------");
        }
    }
}