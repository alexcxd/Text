using System;
using System.Dynamic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.CSharpAdvancedFeatures
{
    /// <summary>
    /// 动态绑定
    /// </summary>
    [TestFixture]
    public class DynamicBindingTest
    {
        /// <summary>
        /// 动态绑定 基本操作
        /// </summary>
        [Test]
        public void DynamicBindingCodeTest()
        {
            //动态绑定是指将绑定(解析类型、成员和操作的过程)的过程从编译时延迟到运行时
            //动态类型通过dynamic声明
            //dynamic类似于object, 是一种不具备描述性的类型, 不同在于dynamic类型在编译时不进行绑定
            dynamic d = 1;
            Console.WriteLine(d.GetType());

            //和动态绑定相对应的是静态绑定, 即不使用动态类型时的绑定方式
            //静态绑定是指由编译器编译, 且依赖于已知类型的绑定方式
            var d1 = 1;
            Console.WriteLine(d.GetType());
        }

        /// <summary>
        /// 动态绑定 类型
        /// </summary>
        [Test]
        public void DynamicBindingTypeTest()
        {
            //动态绑定分为两种类型: 自定义绑定和语言绑定

            //自定义绑定是通过动态对象实现IDynamicMetaObjectProvider(IDMOP)接口实现的
            //Duck只实现了Quack方法没有实现Waddle方法, 也就是说它是用来自定义绑定来拦截所有未实现的方法
            dynamic duck = new Duck();
            duck.Quack();   //1
            duck.Waddle();  //Waddle model was called

            //语言绑定是对象未实现IDynamicMetaObjectProvider时发生的
            //动态绑定会损害静态类型的安全性, 但不会影响运行时类型的安全
            //例如下述方法通过动态类型实现数字类型的通用方法
            int x = 3, y = 4;
            var result = Mean(x, y);
            Console.WriteLine(result);
        }

        static dynamic Mean(dynamic x, dynamic y) => x + y;

        /// <summary>
        /// 动态绑定 异常
        /// </summary>
        [Test]
        public void DynamicBindingException()
        {
            //如果成员绑定失败, 会抛出RuntimeBinderException异常
            dynamic d = 5;
            d.Hello();  //抛出RuntimeBinderException异常
        }

        /// <summary>
        /// 动态绑定 动态类型的运行时表示
        /// </summary>
        [Test]
        public void DynamicBindingRuntime()
        {
            //dynamic和object之间有深度的等价关系, 在运行时typeof(dynamic) == typeof(object)为true
            //动态引用和对象引用类型, 都可以指向除指针以外的任何类型的对象
            dynamic x = "hello";
            Console.WriteLine(x.GetType());   //String
            x = 123;
            Console.WriteLine(x.GetType());   //Int32

            //在结构上object引用和动态引用没有任何区别
            object o = new StringBuilder();
            dynamic d = o;
            d.Append("Hello");
            Console.WriteLine(d);   //Hello

            //如果在一个提供了公开的dynamic成员的类型上使用反射, 会发现这些成员都是标记了特性的object
            var type = typeof(PublicDynamicTest);
            var properties = type.GetProperties();
            foreach (var propertie in properties)
            {
                var attributes = propertie.CustomAttributes;
                foreach (var attribute in attributes)
                {
                    Console.WriteLine(attribute.AttributeType);
                }
            }
        }

        /// <summary>
        /// 动态绑定 动态转换
        /// </summary>
        [Test]
        public void DynamicBindingConversion()
        {

        }
    }

    public class Duck : DynamicObject
    {
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            Console.WriteLine(binder.Name + "model was called");
            result = null;
            return true;
        }

        public void Quack()
        {
            Console.WriteLine(1);
        }
    }

    public class PublicDynamicTest
    {
        public dynamic Foo { get; set; }
    }
}