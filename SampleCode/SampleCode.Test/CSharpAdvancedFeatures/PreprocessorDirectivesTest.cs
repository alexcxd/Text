using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace SampleCode.Test.CSharpAdvancedFeatures
{

    /// <summary>
    /// 预处理指令
    /// </summary>
    [TestFixture]
    public class PreprocessorDirectivesTest
    {
        #region 预处理指令

        /*
         * 预处理                              操作
         * #define symbol                      定义symbol符号(必须定义在using前面)
         * #undef symbol                       取消symbol符号定义(必须定义在using前面)
         * #if symbol [operator symbol2]       判断symbol符号, operator是指运算符, #if后可跟#else、#elif 、#endif
         * #else                               执行到系一个#endif之间的代码
         * #elif symbol [operator symbol2]     组合#else分支和#if的判断
         * #endif                              结束条件语句
         * #warning text                       在编译器输出显示text警告信息(编译的时候在输出内显示)
         * #error text                         在编译器输出显示text错误信息(编译的时候在输出内显示)
         * #pragma warning [disable|restore]   禁用/恢复编译器警告
         * #line [number("file")] | hidden     number是源代码的行号; file是输出的文件名;
         *                                     hidden指示调试器忽略此处到下一个#line指令之间的代码
         * #region name                        标记大纲的开始位置
         * #endregion                          结束一个大纲区域
         */

        #endregion

        /// <summary>
        /// 预处理指令 基本操作
        /// </summary>
        [Test]
        public void PreprocessorDirectivesCodeTest()
        {
            //预编译指令是用于向编译器提供一段代码的附加信息
            //在DEBUG模式下输出DEBUG, 在Release模式下输出Release
#if DEBUG
            Console.WriteLine("DEBUG");
#else
            Console.WriteLine("Release");
#endif

            //结果在输出内看
#line 200 "Special"
            int i;    // CS0168 on line 200
            int j;    // CS0168 on line 201
#line default
            char c;   // CS0168 on line 9
            float f;  // CS0168 on line 10
#line hidden // numbering not affected
            string s;
            double d; // CS0168 on line 13

            Console.WriteLine("Normal line #1."); // Set break point here.
#line hidden
            Console.WriteLine("Hidden line.");
#line default
            Console.WriteLine("Normal line #2.");

        }

        /// <summary>
        /// 预处理指令 Conditional特性
        /// </summary>
        [Test]
        [ConditionalTest]
        public void PreprocessorDirectivesConditionalTest()
        {
            //使用Conditional特性修饰的特性只有在给定的预处理符号出现时才编译
            var method = MethodBase.GetCurrentMethod();
            var attributes = method.GetCustomAttributes(true);

            //只有在debug模式下才会输出特性ConditionalTestAttribute
            foreach (var objectAttribute in attributes)
            {
                var attribute = objectAttribute as Attribute;
                Console.WriteLine(attribute.TypeId);
            }
        }

        
        #pragma warning disable 414
        static string Message = "Hello";
        #pragma warning restore 414

        /// <summary>
        /// 预处理指令 Pragma警告
        /// </summary>
        [Test]
        public void PreprocessorDirectivesPragmaTest()
        {
            //编译器可以通过#pragma warning指令有选择的避免一些警告
            //例如Message字段在没有使用时不要产生警告(警告在输出内)
        }
    }

    [Conditional("DEBUG")]
    public class ConditionalTestAttribute : Attribute
    {

    }
}