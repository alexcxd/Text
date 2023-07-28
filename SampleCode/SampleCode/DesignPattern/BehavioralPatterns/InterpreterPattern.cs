using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SampleCode.DesignPattern.BehavioralPatterns
{
    /// <summary>
    /// 解释器模式
    /// </summary>
    /// 给定一个语言，定义它的语法的一种表示，并定义一种解释器，这个解释器使用该表示来解释语言中的句子
    public class InterpreterPattern
    {
        public static void InterpreterPatternMain()
        {
            Context2 context = new Context2();
            List<Expression1> expressions = new List<Expression1>();

            expressions.Add(new TerminalExpression());
            expressions.Add(new NonterminalExpression());
            expressions.Add(new TerminalExpression());
            expressions.Add(new TerminalExpression());

            foreach(var expression in expressions)
            {
                expression.Interpret(context);
            }

            var a = new List<int>();
        }

        /// <summary>
        /// 中文数字转换阿拉伯数字
        /// </summary>
        public static void ChinaNumberConvert()
        {
            string roman = "五亿七千三百零二万六千四百五十二";
            //分解：((五)亿)((七千)(三百)(零)(二)万)
            //((六千)(四百)(五十)(二))

            var context = new Context1(roman);
            var tree = new ArrayList();

            tree.Add(new GeExpression());
            tree.Add(new ShiExpression());
            tree.Add(new BaiExpression());
            tree.Add(new QianExpression());
            tree.Add(new WanExpression());
            tree.Add(new YiExpression());

            foreach (Expression exp in tree)
            {
                exp.Interpreter(context);
            }

            Console.Write(context.Data);

            Console.Read();
        }

        #region 解释器模式

        public abstract class Expression1
        {
            public abstract void Interpret(Context2 context);
        }

        public class TerminalExpression : Expression1
        {
            public override void Interpret(Context2 context)
            {
                Console.WriteLine("终端解释器");
            }
        }

        public class NonterminalExpression : Expression1
        {
            public override void Interpret(Context2 context)
            {
                Console.WriteLine("非终端解释器");
            }
        }

        public class Context2
        {
            public string Input { get; set; }
            public string Output { get; set; }
        }

        #endregion

        #region 中文数字转换阿拉伯数字

        /// <summary>
        /// 抽象解释器
        /// 包含非终结表达式，内部table的体现
        /// </summary>
        public abstract class Expression
        {
            protected Dictionary<string, int> table = new Dictionary<string, int>(9);

            protected Expression()
            {
                table.Add("一", 1);
                table.Add("二", 2);
                table.Add("三", 3);
                table.Add("四", 4);
                table.Add("五", 5);
                table.Add("六", 6);
                table.Add("七", 7);
                table.Add("八", 8);
                table.Add("九", 9);
            }

            public virtual void Interpreter(Context1 context1)
            {
                if (context1.Statement.Length == 0)
                {
                    return;
                }

                foreach (var key in table.Keys)
                {
                    var value = table[key];

                    if (context1.Statement.EndsWith(key + GetPostFix()))
                    {
                        context1.Data += value * this.Multiplier();
                        context1.Statement = context1.Statement.Substring(0, context1.Statement.Length - this.GetLength());
                    }
                    if (context1.Statement.EndsWith("零"))
                    {
                        context1.Statement = context1.Statement.Substring(0, context1.Statement.Length - 1);
                    }
                }
            }

            public abstract string GetPostFix();

            public abstract int Multiplier();

            //这个可以通用，但是对于个位数字例外，所以用虚方法
            public virtual int GetLength()
            {
                return this.GetPostFix().Length + 1;
            }
        }

        /// <summary>
        /// 个位解释器
        /// 终结表达式
        /// </summary>
        public sealed class GeExpression : Expression
        {
            public override string GetPostFix()
            {
                return "";
            }

            public override int Multiplier()
            {
                return 1;
            }

            public override int GetLength()
            {
                return 1;
            }
        }

        /// <summary>
        /// 十位表达式
        /// </summary>
        public sealed class ShiExpression : Expression
        {
            public override string GetPostFix()
            {
                return "十";
            }

            public override int Multiplier()
            {
                return 10;
            }
        }

        /// <summary>
        /// 百位表达式
        /// </summary>
        public sealed class BaiExpression : Expression
        {
            public override string GetPostFix()
            {
                return "百";
            }

            public override int Multiplier()
            {
                return 100;
            }
        }

        /// <summary>
        /// 千位表达式
        /// </summary>
        public sealed class QianExpression : Expression
        {
            public override string GetPostFix()
            {
                return "千";
            }

            public override int Multiplier()
            {
                return 1000;
            }
        }

        /// <summary>
        /// 万位表达式
        /// </summary>
        public sealed class WanExpression : Expression
        {
            public override string GetPostFix()
            {
                return "万";
            }

            public override int Multiplier()
            {
                return 10000;
            }

            public override void Interpreter(Context1 Context1)
            {
                if (Context1.Statement.Length == 0)
                {
                    return;
                }

                ArrayList tree = new ArrayList();

                tree.Add(new GeExpression());
                tree.Add(new ShiExpression());
                tree.Add(new BaiExpression());
                tree.Add(new QianExpression());

                foreach (string key in table.Keys)
                {
                    if (Context1.Statement.EndsWith(GetPostFix()))
                    {
                        int temp = Context1.Data;
                        Context1.Data = 0;

                        Context1.Statement = Context1.Statement.Substring(0, Context1.Statement.Length - this.GetLength());

                        foreach (Expression exp in tree)
                        {
                            exp.Interpreter(Context1);
                        }
                        Context1.Data = temp + Context1.Data * this.Multiplier();
                    }
                }
            }
        }

        /// <summary>
        /// 亿位表达式
        /// </summary>
        public sealed class YiExpression : Expression
        {
            public override string GetPostFix()
            {
                return "亿";
            }

            public override int Multiplier()
            {
                return 100000000;
            }

            public override void Interpreter(Context1 Context1)
            {
                ArrayList tree = new ArrayList();

                tree.Add(new GeExpression());
                tree.Add(new ShiExpression());
                tree.Add(new BaiExpression());
                tree.Add(new QianExpression());

                foreach (string key in table.Keys)
                {
                    if (Context1.Statement.EndsWith(GetPostFix()))
                    {
                        int temp = Context1.Data;
                        Context1.Data = 0;
                        Context1.Statement = Context1.Statement.Substring(0, Context1.Statement.Length - this.GetLength());

                        foreach (Expression exp in tree)
                        {
                            exp.Interpreter(Context1);
                        }
                        Context1.Data = temp + Context1.Data * this.Multiplier();
                    }
                }
            }
        }

        /// <summary>
        /// 环境上下文
        /// </summary>
        public sealed class Context1
        {
            public Context1(string statement)
            {
                this.Statement = statement;
            }

            public string Statement { get; set; }

            public int Data { get; set; }
        }

        #endregion
    }

}
