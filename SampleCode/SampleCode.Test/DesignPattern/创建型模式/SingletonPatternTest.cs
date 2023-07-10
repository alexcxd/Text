using System;
using NUnit.Framework;
using SampleCode.DesignPattern.CreationalPatterns;
using SampleCode.DesignPattern.CreationalPatterns.Singleton;

namespace SampleCode.Test.DesignPattern.CreationalPatterns
{
    public class SingletonPatternTest
    {
        /// <summary>
        /// 单例模式
        /// </summary>
        [Test]
        public void SingletonPatternCodeTest()
        {
            var instanse = NormalSingleton.GetSingleton();
            var instanse1 = SafeLazySingleton.GetInstance();

            Console.WriteLine(instanse.Equals(instanse1));
        }

    }
}
