using System;
using NUnit.Framework;

namespace SampleCode.Test.CSharpAdvancedFeatures
{
    /// <summary>
    /// Lambda表达式
    /// </summary>
    [TestFixture]
    public class LambdaTest
    {
        delegate int Transformer(int i);

        /// <summary>
        /// Lambda表达式基本操作
        /// </summary>
        [Test]
        public void LambdaCodeTest()
        {
            //Lambda表达式是一种可以替换委托实例的一种匿名方法(通常和Action和Func一起使用)
            //编译器会将Lambda表达式转化成两种形式
            //1.一个委托实例
            //2.一个类型为Expression<TDelegate>的表达式树

            //Lambda表达式的每一个参数对应委托的一个参数, 而表达式的类型对应着委托的返回值(可以为void)
            Transformer t = x => x * x;
            Console.WriteLine(t(3));    //9
            //语句块的写法, 与上述等价
            Transformer t1 = x => { return x * x; };

            //显式指定Lambda表达式参数类型
            static void Foo<T>(T x) { }
            static void Bar<T>(Action<T> x) { }
            Bar((int x) => Foo(x)); //这里必须指定类型, 否则无法通过编译
            Bar<int>(x => Foo(x));
            Bar<int>(Foo);
        }

        /// <summary>
        /// Lambda表达式捕获外部变量
        /// </summary>
        [Test]
        public void LambdaOuterVariableTest()
        {
            //Lambda表达式可以引用方法内的局部变量和方法的参数
            //Lambda表达式所引用的外部变量被称为捕获变量, 捕获变量的表达式称为闭包
            var factor = 2;
            Func<int, int> multiplier = x => x * factor;
            Console.WriteLine(multiplier(3));   //6

            //捕获变量会在委托调用式赋值, 而不是在变量捕获时
            factor = 10;
            Console.WriteLine(multiplier(3));   //30

            //Lambda表达式可以更新捕获变量
            var seed = 0;
            Func<int> natural = () => seed++;
            Console.WriteLine(natural());   //0
            Console.WriteLine(natural());   //1
            Console.WriteLine(seed);   //2

            //捕获变量的生命周期和委托的生命周期一致
            //内部捕获变量是通过将变量提升为私有字段的方法实现,
            //当调用该方法时实例化私有类, 并将其生命周期绑定在委托上
            static Func<int> Natural1()
            {
                var i = 0;
                return () => i++;
            }
            var natural1 = Natural1();
            Console.WriteLine(natural1());   //0
            Console.WriteLine(natural1());   //1

            //在Lambda表达式内实例化的局部变量在每一次调用结束都会释放
            static Func<int> Natural2()
            {
                return () =>
                {
                    var i = 0;
                    return i++;
                };
            }
            var natural2 = Natural2();
            Console.WriteLine(natural2());   //0
            Console.WriteLine(natural2());   //0
        }
    }
}