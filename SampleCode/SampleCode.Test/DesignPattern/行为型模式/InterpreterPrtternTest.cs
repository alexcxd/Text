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
            InterpreterPattern.Context2 context = new InterpreterPattern.Context2();
            List<InterpreterPattern.Expression1> expressions = new List<InterpreterPattern.Expression1>();

            expressions.Add(new InterpreterPattern.TerminalExpression());
            expressions.Add(new InterpreterPattern.NonterminalExpression());
            expressions.Add(new InterpreterPattern.TerminalExpression());
            expressions.Add(new InterpreterPattern.TerminalExpression());

            foreach (var expression in expressions)
            {
                expression.Interpret(context);
            }
        }
    }
}