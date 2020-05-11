using System;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.CSharpAdvancedFeatures
{
    /// <summary>
    /// 匿名类型
    /// </summary>
    [TestFixture]
    public class AnonymousTypesTest
    {
        /// <summary>
        /// 匿名类型基本操作
        /// </summary>
        [Test]
        public void AnonymousTypesCodeTest()
        {
            //匿名类型是一个由编译器临时创建来存储一组值的简单类
            //创建匿名类型可以通过new加上对象初始化器指定类型包含的属性和值
            //匿名类型只能通过var或dynamic进行引用
            //以下匿名类型会被编译器转化为类似AnonymousGeneratedTypeName的类
            var dudu = new
            {
                Name = "Bob",
                Age = 23
            };
            //若在同一个程序集声明两个元素名称和类型都相同的匿名类型, 那么它们在内部就是相同的类型
            var a1 = new {X = 1, Y = 2};
            var a2 = new {X = 3, Y = 4};
            Console.WriteLine(a1.GetType() == a2.GetType());    //True
        }
    }

    public class AnonymousGeneratedTypeName
    {
        public AnonymousGeneratedTypeName(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }

        public string Name { get; }
        public int Age { get; }
        
        //重写Equals和GetHashCode方法

        //重写ToString方法
    }
}