using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.CSharpAdvancedFeatures
{
    /// <summary>
    /// 特性
    /// </summary>
    [TestFixture]
    public class AttributeTest
    {
        #region 特性-定义

        /// <summary>
        /// 特性-定义
        /// </summary>
        [Test]
        public void AttributeBaseTest()
        {
            //C#有三类特性: 位映射特性、伪自定义特性和自定义特性

            //位映射特性
            //位映射特性可以映射到类型元数据的特定位上, 大多数的C#关键字修饰符(public等)都会编译为位映射特性
            //位映射特性在元数据中占据空间很小(大部分仅占据1位)
            //反射API可以通过Type的特定属性访问这些信息(如IsPublic)
            var ta = typeof(Bar).Attributes;    //Attributes仅返回位映射特性和伪自定义特性
            Console.WriteLine(ta);

            //自定义特性
            //自定义特性是通过直接或者间接继承抽象类型System.Attribute的方法定义的
            //自定义特性会被编译为主元数据表的二进制数据

            //伪自定义特性
            //伪自定义特性和自定义特性类似, 都是System和Atribute的子类,
            //区别在于伪自定义特性会在编译器或CLR中优化为位映射特性
        }

        [Obsolete]
        [Serializable]
        public class Bar
        {

        }

        #endregion

        #region 自定义特性-基本操作

        /// <summary>
        /// 自定义特性-基本操作
        /// </summary>
        [Test]
        public void CustomAttributeCodeTest()
        {
            //自定义特性是通过直接或者间接继承抽象类型System.Attribute的方法定义的
            //自定义特性通过方括号指定特性名称并标记代码元素, 例如 [Test]
            //按照惯例, 所有特性类型都已Attribute结尾, 而在附加特性时可以忽略Attribute

            //自定义特性的参数分为两类: 位置参数和命名参数, 例如 [XmlElement("Customer", Namespace = "")]
            //第一个参数为位置参数, 对应的是特性公有构造器中的参数
            //第二个参数为命名参数, 对应的是特性公有字段或公有属性

            //在一个代码元素上可以使用多个特性
            //可以在一个方括号用逗号间隔或者使用多个方括号

            //调用者信息
            //从C# 5 以后可以通过在参数上加以下三个特性来获取调用者的相关信息(编译器直接给默认值)
            // [CallerMemberName]
            // [CallerFilePath]
            // [CallerLineNumber]
            Foo();
        }

        public void Foo([CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            Console.WriteLine("CallerMemberName:" + memberName);
            Console.WriteLine("CallerFilePath:" + filePath);
            Console.WriteLine("CallerLineNumber:" + lineNumber);
        }

        public class CustomerEntity
        {
            [XmlElement("Customer", Namespace = "")]
            public string Name { get; set; }
        }

        #endregion

        #region 自定义特性-定义自定义特性

        /// <summary>
        /// 自定义特性-定义自定义特性
        /// </summary>
        [Test]
        public void CustomAttributeDefineTest()
        {
            //自定义特性的编写方式
            //1.创建一个继承自Attribute类或Attribute子类的类, 按照惯例使用Attribute结尾(非必须)
            //2.使用特性AttributeUsage定义特性的相关信息
            //3.编写一个或多个公用构造函数, 这些构造函数的参数定义了该特性的预留参数, 在使用特性时必须提供
            //4.提供一个或多个公有字段或属性, 在使用特性时可选
            //其中自定义特性的构造器参数和属性只能使用密封的基元类型、Type类型、枚举类型以及以上类型的一维数组
            [CustomTest(1, Message = "")]
            void CustomTestMethod()
            {

            }

            //AttributeUsage特性
            //AttributeUsage特性是一种应用于特性上的特性, 用于告诉编译器如何使用目标特性
            //常用属性: 1.AllowMultiple属性可以控制该特性是否能在相同的目标使用多次
            //          2.Inherited属性可以控制是否能将特性应用到基类型上的同时也应用到子类型上
            //          3.ValidOn属性可以确定附加到哪些目标(类、方法、属性、接口、参数等)上
            //CustomTestAttribute特性定义为仅可在方法上使用, 且可以在相同的目标使用多次

        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
        public sealed class CustomTestAttribute : Attribute
        {
            private int data;

            public string Message { get; set; }

            public CustomTestAttribute(int data)
            {
                this.data = data;
            }
        } 

        #endregion
    }

}

