using System;
using NUnit.Framework;
using SampleCode.DesignPattern.创建型模式;

namespace SampleCode.Test.DesignPattern.创建型模式
{
    public class SingletonPatternTest
    {
        /// <summary>
        /// 单例模式
        /// </summary>
        [Test]
        public void SingletonPatternCodeTest()
        {
            var instanse = Singleton.GetSingleton();
            var instanse1 = Singleton.GetSingleton();

            Console.WriteLine(instanse.Equals(instanse1));
        }

    }
}
