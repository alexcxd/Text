using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SampleCode.Reflect;

namespace SampleCode.Test.CSharpAdvancedFeatures
{
    /// <summary>
    /// 值元组
    /// </summary>
    [TestFixture]
    public class ValueTupleTest
    {
        /// <summary>
        /// 值元组 基本操作
        /// 下简称元组
        /// </summary>
        [Test]
        public void ValueTupleCodeTest()
        {
            //元组用于存储一组值的便捷方法, 主要目的是不使用out从方法返回多个值
            //创建元组的方法是在括号中列出期望值
            var bob = ("Bob", 12);          //等价于(string, int) bob = ("Bob", 12)
            Console.WriteLine(bob.Item1);   //Bob
            Console.WriteLine(bob.Item2);   //12

            //元组是值类型(基于System.ValueTuple泛型结构体), 并且它是可变(可读可写)的元素
            bob.Item1 = "Joe";
            Console.WriteLine(bob.Item1);   //Joe

            //元组可以对元素进行命名
            //类型擦除(见C#7.0核心技术指南4.10.1) 无法重现
            var tuple = (Name: "Bob", Age: 12);
            Console.WriteLine(tuple.Name);   //Bob
            Console.WriteLine(tuple.Age);   //12

            //如果元组对应的元素类型相同, 则元组类型是兼容的
            (string Name, int Age) tuple1 = tuple;

            //可以通过非泛型的ValueTuple类型上调用工厂创建元组
            //这种方法无法对元素进行命名
            var bob2 = ValueTuple.Create("Bob", 12);

            //元组的解构
            //元组支持将一个元组解构为独立的变量
            var (name, age) = ("Bob", 12);
            Console.WriteLine(name);   //Bob
            Console.WriteLine(age);   //12

            //元组的比较
            //ValueTuple实现了Equals方法
            var t1 = ("1", 1);
            var t2 = ("2", 2);
            Console.WriteLine(t1.Equals(t2));   //True
        }
    }
}