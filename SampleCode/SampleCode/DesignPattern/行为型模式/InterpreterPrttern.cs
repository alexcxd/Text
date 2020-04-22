using System;
using System.Collections;
using System.Collections.Generic;

namespace SampleCode.DesignPattern.行为型模式
{
    /// <summary>
    /// 解释器模式
    /// </summary>
    /// 给定一个语言，定义它的语法的一种表示，并定义一种解释器，这个解释器使用该表示来解释语言中的句子
    public class InterpreterPrttern
    {
        public static void InterpreterPrtternMain()
        {
            Context2 context = new Context2();
            List<Expression１> expressions = new List<Expression１>();

            expressions.Add(new TerminalExpression());
            expressions.Add(new NonterminalExpression());
            expressions.Add(new TerminalExpression());
            expressions.Add(new TerminalExpression());

            foreach(var expression in expressions)
            {
                expression.Interpret(context);
            }
        }
    }

    public abstract class Expression１
    {
        public abstract void Interpret(Context2 context);
    }

    public class TerminalExpression : Expression１
    {
        public override void Interpret(Context2 context)
        {
            Console.WriteLine("终端解释器");
        }
    }

    public class NonterminalExpression : Expression１
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




    #region 中文数字转换阿拉伯数字
    // 抽象表达式
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

        public virtual void Interpreter(Context1 Context1)
        {
            if (Context1.Statement.Length == 0)
            {
                return;
            }

            foreach (string key in table.Keys)
            {
                int value = table[key];

                if (Context1.Statement.EndsWith(key + GetPostFix()))
                {
                    Context1.Data += value * this.Multiplier();
                    Context1.Statement = Context1.Statement.Substring(0, Context1.Statement.Length - this.GetLength());
                }
                if (Context1.Statement.EndsWith("零"))
                {
                    Context1.Statement = Context1.Statement.Substring(0, Context1.Statement.Length - 1);
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

    //个位表达式
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

    //十位表达式
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

    //百位表达式
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

    //千位表达式
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

    //万位表达式
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

    //亿位表达式
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

    //环境上下文
    public sealed class Context1
    {
        private string _statement;
        private int _data;

        public Context1(string statement)
        {
            this._statement = statement;
        }

        public string Statement
        {
            get { return this._statement; }
            set { this._statement = value; }
        }

        public int Data
        {
            get { return this._data; }
            set { this._data = value; }
        }
    }

    class Program
    {
        public static void ProgramMain()
        {
            string roman = "五亿七千三百零二万六千四百五十二";
            //分解：((五)亿)((七千)(三百)(零)(二)万)
            //((六千)(四百)(五十)(二))

            Context1 context = new Context1(roman);
            ArrayList tree = new ArrayList();

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
    }
    #endregion
}
