using NUnit.Framework;
using SampleCode.DesignPattern.结构型模式;

namespace SampleCode.Test.DesignPattern.结构型模式
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