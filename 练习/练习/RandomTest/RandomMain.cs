using System;
using System.Diagnostics;
using System.Threading;

namespace 练习.Random
{
    public class RandomMain
    {
        public static void Write()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            RandomMain randomMain = new RandomMain();
            var random = new System.Random(GetRandomSeedbyTicks());
            for (int i = 0; i < 100000; ++i)
            {
                Console.WriteLine(randomMain.Demo1(random));
            }
            sw.Stop();
            var ts = sw.Elapsed.TotalMilliseconds;
        }

        
        public int Demo1(System.Random random)
        {
            var num = random.Next(111111,999999);
            return num;
        }

        /// <summary>
        /// 使用Tread.sleep生成真随机数，太慢
        /// </summary>
        /// <returns></returns>
        public int Demo2()
        {
            //10万6位随机数耗时：105262.43430000001ms
            var random = new System.Random();
            var num = random.Next(111111, 999999);
            Thread.Sleep(1);
            return num;
        }

        /// <summary>
        /// Random默认使用DateTime.Now.Ticks生成随机数； 
        /// </summary>
        /// 10万6位随机数耗时：6570.0633000000007
        static int GetRandomSeedbyTicks()
        {
            return ~unchecked((int) DateTime.Now.Ticks);
        }

        /// <summary>
        /// 使用Guid生成种子 最快
        /// </summary>
        /// <returns></returns>
        /// 10万6位随机数耗时：6742.6292
        static int GetRandomSeedbyGuid()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int iSeed = BitConverter.ToInt32(buffer, 0);
            return iSeed;
        }

        /// <summary>
        /// 使用RNGCryptoServiceProvider生成种子
        /// </summary>
        /// <returns></returns>
        /// 10万6位随机数耗时7171ms
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);

        }

    }
}