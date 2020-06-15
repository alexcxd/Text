using System;
using System.Runtime.Intrinsics.X86;
using NUnit.Framework;
using SampleCode.Math;

namespace SampleCode.Test.Math
{
    /// <summary>
    /// 随机
    /// </summary>
    [TestFixture]
    public class RandomHelperTest
    {
        /// <summary>
        /// Random基本操作
        /// </summary>
        [Test]
        public void RandomCodeTest()
        {
            //Random类可以生成类型为byte、integer或double生成随机数序列
            //实例化时, 可以选择传入一个种子(若不传以当前系统时间作为种子)
            //使用相同的种子一定会产生相同序列的数字(重现性)
            var romdom1 = new Random(1);
            var romdom2 = new Random(1);
            var i1 = romdom1.Next(100); 
            var i2 = romdom2.Next(100);
            var b1 = i1 == i2;             //true
            var i3 = romdom1.Next(100);
            var i4 = romdom2.Next(100);
            var b2 = i3 == i4;             //true
        }


        /// <summary>
        /// 随机数字
        /// </summary>
        [Test]
        public void RandomNumberTest()
        {
            var random = new RandomHelper(RandomSeed.GetRandomSeedByRngCrypto());
            var number = random.RandomNumber(0, 1000);
            Console.WriteLine(number);
        }

        /// <summary>
        /// 随机字符串
        /// </summary>
        [Test]
        public void RandomStringTest()
        {
            var random = new RandomHelper(RandomSeed.GetRandomSeedByRngCrypto());
            var str = random.RandomString(10);
            Console.WriteLine(str);
        }
    }
}