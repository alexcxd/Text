using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.RegexMatch
{
    /// <summary>
    /// 正则表达式基础
    /// 完整正则表达式语法特性 见C#7.0核心技术指南26.7
    /// </summary>
    [TestFixture]
    public class RegexBasisTest
    {
        #region 字符集合

        // 表达式      含义                                                 反义表达式
        // [abcdef]    匹配括号中的某一个字符                               [^abcdef]
        // [a-f]       匹配制定范围内的某一个字符                           [^a-f]
        // \d          等价于[0-9]                                          \D
        // \w          匹配一个单词字符(会根据CultureInfo.CurrentCulture    \W
        //             发生变化, 在英语环境下等价于[a-zA-Z_0-9])
        // \s          匹配一个空白字符。等价[\n\r\t\f\v]                   \S
        // p{category} 匹配category中限定的字符类型                         \P
        // .           默认模式下匹配所有字符，但是不匹配\n(StringLine模式  \n
        //             下匹配所有字符)

        #endregion

        #region 量词符号

        // 量词符号     含义
        // *            0次或者多次匹配
        // +            1次或者多次匹配
        // ?            0次或者1次匹配
        // {n}          n次匹配
        // {n,}         至少n次匹配
        // {n,m}        n到m次匹配

        #endregion

        #region RegexOptions属性

        // 枚举值                  正则表达式选项字母  介绍
        // IgnoreCase              i                   忽略大小写(默认情况下是区分大小写的)
        // Multiline               m                   修改^和$的语义，使其匹配一行的开始和结尾
        // ExplicitCapture         n                   捕获显式命名或显示指定编号的组 详见分组
        // Compiled                                    强制将正则表达式编译为IL代码
        // SingleLine              s                   确保.符号匹配所有字符(包括\n)
        // IgnorePatternWhitespace  x                   忽略所有未转义的空白字符串
        // RightToLeft             r                   从右向左搜索, 无法在表达式的中间应用该选项
        // ECMAScript                                  强制符合ECMA标准
        // CultureInvariant                            在字符串比较时不使用文化相关的比较规则

        #endregion

        #region 匹配

        /// <summary>
        /// 正则表达式-匹配基本
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
            //其他RegexOptions枚举值对应的简写见C#7.0核心技术指南
            Console.WriteLine(Regex.Match("a", @"A", RegexOptions.IgnoreCase));
            Console.WriteLine(Regex.Match("a", @"(?i)A"));
            Console.WriteLine(Regex.Match("AAAa", @"(?i)A(?-i)a"));

            //字符转义:如果以下字符(·\*+?|{[()^$.#)不在一个方括号中不会作为字面量来处理，使用需要转义
            //例如若要匹配?应使用\?而不是?
            Console.WriteLine(Regex.IsMatch("what?", @"what\?") + "," + Regex.IsMatch("what?", @"what?"));
            //可以使用Regex.Escape将元字符的字符串替换为转义形式，Regex.Unescape方法相反
            Console.WriteLine("Escape:" + Regex.Escape(@"?"));      //\?
            Console.WriteLine("Unescape:" + Regex.Unescape(@"\?"));   // ?

            //懒惰量词符号和贪婪量词符号
            //默认情况下量词都是贪婪的
            var html = @"<i> By default</i> quantifiers are <i> greedy</i> creatures";
            //懒惰量词符号会尽可能少的匹配, 通过在量词后加?使用
            Console.WriteLine(Regex.Match(html, @"<i>.*?</i>"));     //<i> By default</ i>
            //贪婪量词符号会尽可能多的匹配重复项目
            Console.WriteLine(Regex.Match(html, @"<i>.*</i>"));      //<i> By default</i> quantifiers are <i> greedy</i>
        }

        /// <summary>
        /// 前向条件和后向条件
        /// </summary>
        [Test]
        public void RegexLookbehindAndLookaheadTest()
        {
            //正前向条件(?=expr)结构可用于检查后续文本是否与expr匹配, 而expr本身不作为结果的一部分
            Console.WriteLine(Regex.Match("say 25 miles more", @"\d+\s(?=miles)")); //25
            //使用正向前条件校验密码强度,至少6位字符且至少有一个数字
            Console.WriteLine(Regex.IsMatch("15asd1", @"(?=.*\d).{6,}"));

            //负前向条件(?!expr)要求后续文本与expr不匹配
            Console.WriteLine(Regex.IsMatch("Good work! But...", @"(?i)good(?!.*(however|but))")); //false
            Console.WriteLine(Regex.IsMatch("Good work! Then...", @"(?i)good(?!.*(however|but))")); //true

            //正后向条件(?<=expr)要求匹配之前要出现指定表达式的内容
            Console.WriteLine(Regex.IsMatch("However good, we...", @"(?i)(?<=however.*)good")); //true
            Console.WriteLine(Regex.IsMatch("Very good, we...", @"(?i)(?<=however.*)good")); //false

            //正后向条件(?<!expr)要求匹配之前要不出现指定表达式的内容
            Console.WriteLine(Regex.IsMatch("However good, we...", @"(?i)(?<!however.*)good")); //false
            Console.WriteLine(Regex.IsMatch("Very good, we...", @"(?i)(?<!however.*)good")); //true
        }

        /// <summary>
        /// 锚点
        /// </summary>
        [Test]
        public void RegexAnchorsTest()
        {
            //锚点(anchor)^和$代表确定的位置。^匹配字符串的开头, $匹配字符串的结尾
            Console.WriteLine(Regex.Match("Not Now", @"^[Nn]o"));   //No
            Console.WriteLine(Regex.Match("f = 0.2F", @"[Ff]$"));   //F

            //如果是在RegexOptions.Multiline模式下使用锚点
            //^匹配字符串的开始或者行的开始(紧邻在\n之后)
            //$匹配字符串的结束或者行的结束(紧邻在\n之前)
            var fileNames = "a.txt" + "\r\n" + "b.doc" + "\r\n" + "c.txt";
            var matches = Regex.Matches(fileNames, @".+\.txt(?=\r?$)", RegexOptions.Multiline);
            foreach (var m in matches)
            {
                Console.Write(m + " ");
            }

            //由于锚点匹配的是一个位子而不是一个字符, 使用锚点匹配得到的是一个空字符串
            Console.WriteLine(Regex.Match("x", @"$"));
        }

        /// <summary>
        /// 单词边界
        /// </summary>
        [Test]
        public void RegexWordBoundariesTest()
        {
            //单词边界\b匹配一个或者多个\w毗邻的位置
            //\b常用于匹配整个单词
            var matches = Regex.Matches("Wedding in Sarajevo", @"\b\w+\b");
            foreach (var match in matches)
            {
                Console.Write(match + "   ");
            }
        }

        /// <summary>
        /// 分组
        /// </summary>
        [Test]
        public void RegexWGroupsTest()
        {
            //可以通过括号对匹配的结果进行分组
            //其中match.Groups中的第一个为完整的匹配结果
            var match = Regex.Match("206-567-788", @"(\d+-\d+)-(\d+)");
            Console.WriteLine(match.Groups[0]);
            Console.WriteLine(match.Groups[1]);
            Console.WriteLine(match.Groups[2]);

            //引用分组, 在正则表达式中可以通过\n来引用索引为n的分组
            var matches = Regex.Matches("pop pope peep", @"\b(\w)\w+\1\b");
            foreach (var m in matches)
            {
                Console.Write(m + "   ");
            }

            //命名分组
            //命名规则 (? 'group-name' group-expr) OR (? <group-name> group-expr)
            //引用规则 \k'group-name' OR \k<group-name>
            var match1 = Regex.Match("<h1>Hello</h1>", @"<(?<tag>\w+?)>(?<text>.*?)</\k<tag>>");
            Console.WriteLine(match1.Value);
            Console.WriteLine(match1.Groups["tag"]);    //h1
            Console.WriteLine(match1.Groups["text"]);   //Hello
        }

        #endregion

        #region 替换

        /// <summary>
        /// 正则表达式-替换基本
        /// </summary>
        [Test]
        public void RegexReplace()
        {
            //Regex.Replace方法可以替换匹配结果
            //例如将单词cat替换为dog
            Console.WriteLine(Regex.Replace("catapult the cat", @"\bcat\b", @"dog"));   //catapult the dog

            //替换字符串可以通过$0作为替换结构访问原始的匹配
            //例如将所有数字都加上尖括号
            Console.WriteLine(Regex.Replace("10 puls 20 makes 30", @"\d+", @"<$0>"));   //<10> puls <20> makes <30>

            //通过$1,$2,$3以此类推的方式捕获任意分组
            Console.WriteLine(Regex.Replace("10 puls 20 makes 30", @"(\d+) (\b\w+\b)", @"<$1> ($2)"));   //<10> (puls) <20> (makes) 30

            //通过分组的命名捕获任意分组
            Console.WriteLine(Regex.Replace(
                "<msg>Hello</msg>", 
                @"<(?<tag>\w+?)>(?<text>.*?)</(\k'tag')>",
                @"<${tag} value = ""${text}""/>"));            //< msg value = "Hello"/>

            //MatchEvaluator委托
            //在Regex.Replace中通过MatchEvaluator委托实现对匹配结果的逻辑替换
            Console.WriteLine(Regex.Replace("5 is less then 10", @"\d+", x => ((int.Parse(x.Value) * 10)).ToString())); //50 is less then 100
        }

        #endregion

        #region 拆分

        /// <summary>
        /// 正则表达式-拆分基本
        /// </summary>
        [Test]
        public void RegexSplitTest()
        {
            //Regex.Split可以通过正则表达式来表示分隔符, 其中结果中不会包含间隔符
            var strList = Regex.Split("a5e3q", @"\d");
            foreach(var str in strList)
            {
                Console.Write(str + " ");
            }

            //若需要将间隔符包含结果中, 可以将表达式放入正前向条件中
            //例如，将使用驼峰命名法的字符串分割为单词
            var strList1 = Regex.Split("oneTwoThree", @"(?=[A-Z])");
            foreach (var str in strList1)
            {
                Console.Write(str + " ");
            }

        }

        #endregion
    }
}