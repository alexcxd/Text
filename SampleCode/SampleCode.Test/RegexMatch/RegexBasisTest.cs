using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.RegexMatch
{
    /// <summary>
    /// 正则表达式基础
    /// </summary>
    [TestFixture]
    public class RegexBasisTest
    {
        /// <summary>
        /// 基本操作-匹配
        /// </summary>
        [Test]
        public void RegexMatchTest()
        {
            //Regex.Match方法可以在一个大型字符串内进行搜索, 获取第一个匹配值。
            //默认情况下正则引擎会按照从左到右的顺序进行匹配
            var m = Regex.Match("any colour you like colour", @"colou?r");
            Console.WriteLine($@"Match Seccess:{m.Success} Index:{m.Index} Lengh:{m.Length}
                                       Value:{m.Value} Value:{m}");

            //NextMatch可以获得第一个匹配值之后的下一个匹配值
            var m2 = m.NextMatch();

            //Regex.Matches方法可以在一个大型字符串内进行搜索, 所有匹配值
            var m3 = Regex.Matches("any colour you like colour", @"colou?r");

            //Regex.IsMatch方法可以在一个大型字符串内进行搜索匹配值是否操作
            var m4 = Regex.IsMatch("Jenny", "Jen(ny|fer)");

            //RegexOptions枚举可以控制正则表达式匹配的行为
            //例1：RegexOptions.IgnoreCase会在匹配中忽视大小写
            //RegexOptions.IgnoreCase可以简写为(?i)，同时可以通过(?-i)关闭
            Console.WriteLine(Regex.Match("a", @"A", RegexOptions.IgnoreCase));
            Console.WriteLine(Regex.Match("a", @"(?i)A"));
            Console.WriteLine(Regex.Match("AAAa", @"(?i)A(?-i)a"));
        }
    }
}