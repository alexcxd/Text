using System;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SampleCode.Random;

namespace SampleCode.Test.Random
{
    /// <summary>
    /// 随机
    /// </summary>
    [TestFixture]
    public class RandomHelperTest
    {
        [Test]
        public void RandomNumberTest()
        {
            var random = new RandomHelper(RandomSeed.GetRandomSeedByRngCrypto());
            var number = random.RandomNumber(0, 1000);
            Console.WriteLine(number);
        }

        [Test]
        public void RandomStringTest()
        {
            var random = new RandomHelper(RandomSeed.GetRandomSeedByRngCrypto());
            var str = random.RandomString(10);
            Console.WriteLine(str);
        }
    }
}