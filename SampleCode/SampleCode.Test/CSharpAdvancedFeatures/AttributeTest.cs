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
        /// <summary>
        /// 特性-基本操作
        /// </summary>
        [Test]
        public void AttributeCodeTest()
        {
            //特性是通过直接或者间接继承抽象类型System.Attribute的方法定义的
            //特性通过方括号指定特性名称并标记代码元素, 例如 [Test]
            //按照惯例, 所有特性类型都已Attribute结尾, 而在附加特性时可以忽略Attribute

            //特性的参数分为两类: 位置参数和命名参数, 例如 [XmlElement("Customer", Namespace = "")]
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
    }

    public class CustomerEntity
    {
        [XmlElement("Customer", Namespace = "")]
        public string Name { get; set; }
    }
}