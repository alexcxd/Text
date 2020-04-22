using System.Collections.Generic;
using NUnit.Framework;
using SampleCode.DesignPattern.行为型模式;

namespace SampleCode.Test.DesignPattern.行为型模式
{
    [TestFixture]
    public class InterpreterPrtternTest
    {
        /// <summary>
        /// 解释器模式
        /// </summary>
        [Test]
        public void InterpreterPrtternCodeTest()
        {
            Context2 context = new Context2();
            List<Expression１> expressions = new List<Expression１>();

            expressions.Add(new TerminalExpression());
            expressions.Add(new NonterminalExpression());
            expressions.Add(new TerminalExpression());
            expressions.Add(new TerminalExpression());

            foreach (var expression in expressions)
            {
                expression.Interpret(context);
            }
        }
    }
}