using System;
using NUnit.Framework;

namespace SampleCode.Test.CSharpAdvancedFeatures
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    [TestFixture]
    public class ExtensionMethodsTest
    {
        /// <summary>
        /// 扩展方法基本操作
        /// </summary>
        [Test]
        public void ExtensionMethodsCodeTest()
        {
            //扩展方法允许在现有类型上扩展新的方法而无需修改原始类的定义
            //扩展方法是定义在静态类的静态方法里,其中第一个参数需要用this修饰且类型为需要扩展的类型
            Console.WriteLine("Abc".IsCapitalized());
            //编译后会转化为普通的静态方法调用，两者等价
            Console.WriteLine(StringHelper.IsCapitalized("Abc"));

            //扩展方法链
            "sausage".Pluralize().Capitalized();

            //二义性
            //1.任何兼容的实例方法的优先级高于扩展方法, 例如StringHelp.SubString的优先级低于String本身的
            var s = "abcdef".Substring(1, 2);   //bc
            //2.如果两个扩展方法签名相同,那么必须使用普通的静态方法调用才能区分
            //  如果其中一个扩展方法具有更具体的参数类型, 那么它的优先级更高
            var s1 = "abcdef".IsCapitalized();
        }
    }

    public static class StringHelper
    {
        public static bool IsCapitalized(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            return char.IsUpper(s[0]);
        }

        public static string Pluralize(this string s)
        {
            return s + "s";
        }

        public static string Capitalized(this string s)
        {
            var c = char.ToUpper(s[0]);
            return c + s.Substring(1, s.Length - 2);
        }

        public static string Substring(this string s, int startIndex, int lenth)
        {
            return "a";
        }
    }

    public static class ObjectHelper
    {
        public static bool IsCapitalized(this object s)
        {
            var str = s as string;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            return char.IsUpper(str[0]);
        }
    }
}