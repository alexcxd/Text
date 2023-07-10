using NUnit.Framework;
using SampleCode.DesignPattern.StructuralPatterns;

namespace SampleCode.Test.DesignPattern.StructuralPatterns
{
    [TestFixture]
    public class FlyweightPatternTest
    {
        /// <summary>
        /// 享元模式
        /// </summary>
        [Test]
        public void FlyweightPatternCodeTest()
        {
            FlyweightFactory factory = new FlyweightFactory();

            var flyweightA1 = factory.GetFlyweight("A");
            flyweightA1.Operation();

            var flyweightA2 = factory.GetFlyweight("A");
        }
    }
}