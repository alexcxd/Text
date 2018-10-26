using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace 练习.Random
{
    public class RandomMain
    {
        private static List<char> charList = new List<char>(){'A','B','C','D','E','F','G','H','I','J','K','L',
                                                       'M','N','O','P','K','R','S','T','U','V','W','S',
                                                       'Y','Z','a','b','c','d','e','f','g','h','i','j',
                                                       'k','l','m','n','o','p','k','r','s','t','u','v',
                                                       'w','s','y','z','1','2','3','4','5','6','7','8',
                                                       '9','0','!','@','#','$','%','^','&','*','(',')',
                                                       '_','+','-','=','{','}','[',']',';',':','<','>'
        };

        public static void Write()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            RandomMain randomMain = new RandomMain();
            var random = new System.Random(GetRandomSeedbyGuid());
            StringBuilder str = new StringBuilder();
            RandomMain rm = new RandomMain();
            for (int i = 0; i < 7; ++i)
            {
                str.Append(charList[rm.Demo1(random)]);
            }
            Console.WriteLine(str);
            sw.Stop();
            var ts = sw.Elapsed.TotalMilliseconds;
        }


        public int Demo1(System.Random random)
        {
            var num = random.Next(0, 83);
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
            return ~unchecked((int)DateTime.Now.Ticks);
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