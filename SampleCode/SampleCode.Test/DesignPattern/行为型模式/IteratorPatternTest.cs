using System;
using NUnit.Framework;
using SampleCode.DesignPattern.BehavioralPatterns;

namespace SampleCode.Test.DesignPattern.BehavioralPatterns
{
    [TestFixture]
    public class IteratorPatternTest
    {
        /// <summary>
        /// 迭代器模式
        /// </summary>
        [Test]
        public void IteratorPatternCodeTest()
        {
            var a = new ConcreteAggregate
            {
                [0] = "1",
                [1] = "2",
                [2] = "3",
                [3] = "4",
                [4] = "5",
                [5] = "6",
                [6] = "7",
                [7] = "8",
                [8] = "9"
            };

            var iterator = a.CreateIterator();

            while (!iterator.IsDone())
            {
                Console.WriteLine("{0}", iterator.Next());
            }
        }
    }
}