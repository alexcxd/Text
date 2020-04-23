using System;
using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SampleCode.Reflect;

namespace SampleCode.Test.Reflect
{
    /// <summary>
    /// 动态类型dynamic和反射的结合使用
    /// </summary>
    [TestFixture()]
    public class DynamicToReflectTest
    {
        [Test]
        public void DynamicToReflectCodeTest()
        {
            var classPath = "SampleCode.Reflect.User";

            /*//Assembly.Load("Lib") 加载的程序集（即要创建的对象类型在哪个程序集中定义）
            var assembly = Assembly.Load("Entity");
            var user = (User)assembly.CreateInstance(classPath1);*/


            //"Lib" 参数指明要加载的程序集（即要创建的对象类型在哪个程序集中定义）
            var handler1 = Activator.CreateInstance("SampleCode", classPath);
            var user = handler1.Unwrap() as User;


            var sw = new Stopwatch();
            sw.Start();

            //使用dynamic可以省去了频繁的类型转换操作。
            int i = 1000000;
            while (i > 0)
            {
                /*//Assembly.Load("Lib") 加载的程序集（即要创建的对象类型在哪个程序集中定义）
                var assembly = Assembly.Load("SampleCode");
                var user = (User)assembly.CreateInstance(classPath);*/

                /*//Assembly.Load("Lib") 加载的程序集（即要创建的对象类型在哪个程序集中定义）
                var assembly = Assembly.Load("SampleCode");
                dynamic b = assembly.CreateInstance(classPath);*/
                i--;
            }

            sw.Stop();
            var time = sw.ElapsedMilliseconds;
        }
    }
}