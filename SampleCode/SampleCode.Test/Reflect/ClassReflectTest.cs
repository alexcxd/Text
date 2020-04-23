using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using SampleCode.Reflect;

namespace SampleCode.Test.Reflect
{
    /// <summary>
    /// 类反射
    /// </summary>
    [TestFixture]
    public class ClassReflectTest
    {
        [Test]
        public void ClassReflectCodeTest()
        {
            //1.初始化
            var intType = typeof(int);
            var stringType = Type.GetType("System.String");//完整名称
            var userType = (new User()).GetType();

            //2.属性
            //判断类型
            Console.WriteLine(intType.IsValueType);
            Console.WriteLine(intType.IsAbstract);
            Console.WriteLine(intType.IsEnum);
            Console.WriteLine(intType.IsPrimitive);
            Console.WriteLine(intType.IsClass);
            //获取对象名，全称，命名空间
            Console.WriteLine(intType.Name + "\n" + intType.FullName + "\n" + intType.Namespace + "\n");
            //基本类型
            Console.WriteLine(intType.BaseType);


            //3.获取类中的构造方法
            var constructors = userType.GetConstructors();
            foreach (var constructor in constructors)
            {
                //获取构造函数所有参数
                var parameters = constructor.GetParameters();
            }
            //通过构造函数创造实例
            var t1 = new[] { typeof(List<int>) };
            var constructor1 = userType.GetConstructor(t1);
            object[] obj = { new List<int>() };
            var newClass = constructor1.Invoke(obj);
            //使用Activator通过构造函数生成实例
            var newClass1 = Activator.CreateInstance(userType, new List<int>());

            //4.获取类的public属性
            PropertyInfo[] properties = userType.GetProperties();

            //5.获取该类所有public方法的详细信息
            MethodInfo[] methods = stringType.GetMethods();

            //6.获取类中的public字段
            FieldInfo[] fields = userType.GetFields();

            //7.获取类的private属性
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            PropertyInfo[] properties1 = userType.GetProperties(flag);

            //8.获取类中的private字段
            FieldInfo[] fields1 = userType.GetFields(flag);

            //用反射生成对象，并调用属性、方法和字段进行操作 
            Type userType1 = Type.GetType("SampleCode.Reflect.User");
            if (userType1 != null)
            {
                //生成对象，public类型可直接通过对象访问，private需要使用反射
                var user = Activator.CreateInstance(userType);
                PropertyInfo fi = userType.GetProperty("Password", flag);
                fi.SetValue(user, "123");
            }
        }
    }
}