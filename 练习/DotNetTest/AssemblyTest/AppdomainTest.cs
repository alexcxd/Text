using System;
using System.Threading;
using DotNetTest.AttributeTest;

namespace DotNetTest.AssemblyTest
{
    /// <summary>
    /// 应用程序域相关
    /// </summary>
    public class AppDomainTest
    {
        public static void AppDomainTestMian()
        {
            new AppDomainTest().Demo2();
        }

        /// <summary>
        /// 创建、加载、卸载应用程序域和获取相关程序集
        /// </summary>
        /// 当加载程序集后，就无法把它从AppDomain中卸载，只能把整个AppDomain卸载。
        public void Demo1()
        {
            //创建一个应用程序域
            var appDomain = AppDomain.CreateDomain("NewAppDomain");
           
            //应用程序域加载程序集时发生的事件
            appDomain.AssemblyLoad += (obj, e) =>
            {
                Console.WriteLine("NewAppDomain Unload!");
            };
            //卸载应用程序域时发生的事件
            appDomain.DomainUnload += (obj, e) =>
            {
                Console.WriteLine("NewAppDomain Unload!");
            };


            //加载程序集
            appDomain.Load("Entity");

            //获取该应用程序域的所有程序集
            foreach (var assembly in appDomain.GetAssemblies())
            {
                Console.WriteLine(string.Format("{0}\n----------------------------",
                    assembly.FullName));
            }

            //卸载应用程序域
            AppDomain.Unload(appDomain);
            Console.ReadKey();
        }

        /// <summary>
        /// 在AppDomain中建立程序集中指定类的对象
        /// </summary>
        public void Demo2()
        {
            var test = AppDomain.CurrentDomain;
            var attribute = AppDomain.CurrentDomain.CreateInstance("DotNetTest", "DotNetTest.AttributeTest.AttributeDemo1").Unwrap() as AttributeDemo1;
            var context = Thread.CurrentContext;
        }
    }
}