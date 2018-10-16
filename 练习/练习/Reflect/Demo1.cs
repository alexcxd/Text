using System;
using System.Collections.Generic;
using System.Reflection;
using 练习.DataStructrue;

namespace 练习.Reflect
{
    public class Demo1
    {
        public static void TypeTest()
        {
            //1.初始化
            Type intType = typeof(int);
            Type intType1 = Type.GetType("System.String");//完整名称
            Type intType2 = intType.GetType();

            //2.属性
            /*//判断类型
            Console.WriteLine(inType.IsValueType);
            Console.WriteLine(inType.IsAbstract);
            Console.WriteLine(inType.IsEnum);
            Console.WriteLine(inType.IsPrimitive);
            Console.WriteLine(inType.IsClass);
            //获取对象名，全称，命名空间
            Console.WriteLine(t.Name + "\n" + t.FullName + "\n" + t.Namespace + "\n");
            //基本类型
            Console.WriteLine(t.BaseType);*/

            Type t = typeof(BinarySortTreeLink);

            //3.获取类中的构造方法
            ConstructorInfo[] constructors = t.GetConstructors();
            foreach (ConstructorInfo constructor in constructors)
            {
                //获取构造函数所有参数
                ParameterInfo[] parameters = constructor.GetParameters();
            }
            //通过构造函数创造实例
            Type[] t1 = new []{typeof(List<int>) };
            ConstructorInfo constructor1 = t.GetConstructor(t1);
            object[] obj = new[] {new List<int>()};
            var newClass = constructor1.Invoke(obj);
            //使用Activator通过构造函数生成实例
            var newClass1 = Activator.CreateInstance(t, new List<int>());

            //4.获取类的public属性
            Type t2 = typeof(User);
            PropertyInfo[] properties = t2.GetProperties();

            //5.获取该类所有public方法的详细信息
            MethodInfo[] methods = t.GetMethods();

            //6.获取类中的public字段
            FieldInfo[] fields = t2.GetFields();

            //7.获取类的private属性
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            PropertyInfo[] properties1 = t2.GetProperties(flag);

            //8.获取类中的private字段
            FieldInfo[] fields1 = t2.GetFields(flag);

            //用反射生成对象，并调用属性、方法和字段进行操作 
            Type userType = Type.GetType("练习.Reflect.User");
            if (userType != null)
            {
                //生成对象，public类型可直接通过对象访问，private需要使用反射
                var user = Activator.CreateInstance(userType);
                PropertyInfo fi = userType.GetProperty("Password", flag);
                fi.SetValue(user,"123");
            }

        }
    }
}