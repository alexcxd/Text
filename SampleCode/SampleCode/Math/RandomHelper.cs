using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SampleCode.Math
{
    public class RandomHelper
    {
        private static readonly List<char> CharList = new List<char>(){
            'A','B','C','D','E','F','G','H','I','J','K','L',
            'M','N','O','P','K','R','S','T','U','V','W','S',
            'Y','Z','a','b','c','d','e','f','g','h','i','j',
            'k','l','m','n','o','p','k','r','s','t','u','v',
            'w','s','y','z','1','2','3','4','5','6','7','8',
            '9','0','!','@','#','$','%','^','&','*','(',')',
            '_','+','-','=','{','}','[',']',';',':','<','>'
        };

        private static System.Random _random;

        public RandomHelper(int send)
        {
            _random = new System.Random(send);
        }

        public int RandomNumber(int minNumber, int maxNumber)
        {
            var num = _random.Next(minNumber, maxNumber);
            return num;
        }

        public string RandomString(int charCount, List<char> charList = null)
        {
            var str = new StringBuilder();

            charList ??= CharList;

            for (var i = 0; i < charCount; ++i)
            {
                str.Append(charList[_random.Next(0, charList.Count - 1)]);
            }

            return str.ToString();
        }
    }

    public static class RandomSeed
    {
        /// <summary>
        /// Random默认使用DateTime.Now.Ticks生成随机数； 
        /// </summary>
        /// 10万6位随机数耗时：6570.0633000000007
        public static int GetRandomSeedbyTicks()
        {
            //unchecked 关键字用于取消整型类型的算术运算和转换的溢出检查。
            return ~unchecked((int)DateTime.Now.Ticks);
        }

        /// <summary>
        /// 使用Guid生成种子 最快
        /// </summary>
        /// <returns></returns>
        /// 10万6位随机数耗时：6742.6292
        public static int GetRandomSeedbyGuid()
        {
            var buffer = Guid.NewGuid().ToByteArray();
            var iSeed = BitConverter.ToInt32(buffer, 0);
            return iSeed;
        }

        /// <summary>
        /// 使用RNGCryptoServiceProvider生成种子
        /// </summary>
        /// <returns></returns>
        /// 10万6位随机数耗时7171ms
        public static int GetRandomSeedByRngCrypto()
        {
            var bytes = new byte[4];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

    }
}