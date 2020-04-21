using DotNetTest.DesignPattern;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleCode.Test.DesignPattern
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
