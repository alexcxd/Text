using System.Collections.Generic;
using NUnit.Framework;
using SampleCode.DesignPattern.BehavioralPatterns;

namespace SampleCode.Test.DesignPattern.BehavioralPatterns
{
    [TestFixture]
    public class InterpreterPatternTest
    {
        /// <summary>
        /// 解释器模式
        /// </summary>
        [Test]
        public void InterpreterPatternCodeTest()
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