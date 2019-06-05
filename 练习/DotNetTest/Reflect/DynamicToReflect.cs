using System;
using System.Diagnostics;
using System.Reflection;

namespace DotNetTest.Reflect
{
    public class DynamicToReflect
    {
        /// <summary>
        /// 测试动态类型dynamic和反射的结合使用
        /// </summary>
        public static void Demo1()
        {
            string classPath1 = "Entity.User";

            /*//Assembly.Load("Lib") 加载的程序集（即要创建的对象类型在哪个程序集中定义）
            var assembly = Assembly.Load("Entity");
            Entity.User b = (Entity.User)assembly.CreateInstance(classPath1);*/


            /*bugcxd//"Lib" 参数指明要加载的程序集（即要创建的对象类型在哪个程序集中定义）
            var handler1 = Activator.CreateInstance("User", classPath1);
            User b1 = handler1.Unwrap() as User;*/


            Stopwatch sw = new Stopwatch();
            sw.Start();

            //使用dynamic可以省去了频繁的类型转换操作。
            int i = 1000000;
            while (i > 0)
            {
                //Assembly.Load("Lib") 加载的程序集（即要创建的对象类型在哪个程序集中定义）
                var assembly = Assembly.Load("Entity");
                Entity.User b = (Entity.User)assembly.CreateInstance(classPath1);

                /*//Assembly.Load("Lib") 加载的程序集（即要创建的对象类型在哪个程序集中定义）
                var assembly = Assembly.Load("Entity");
                dynamic b = assembly.CreateInstance(classPath1);*/
                i--;
            }

            sw.Stop();
            var time = sw.ElapsedMilliseconds;

        }
        
    }
}