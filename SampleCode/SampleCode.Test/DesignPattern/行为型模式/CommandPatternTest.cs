using NUnit.Framework;
using SampleCode.DesignPattern.BehavioralPatterns;

namespace SampleCode.Test.DesignPattern.BehavioralPatterns
{
    [TestFixture]
    public class CommandPatternTest
    {
        /// <summary>
        /// 命令模式
        /// </summary>
        [Test]
        public void CommandPatternCodeTest()
        {
            Receiver receiver = new Receiver();
            ConcreteCommand command = new ConcreteCommand();
            command.SetReceiver(receiver);
            Invoker invoker = new Invoker();
            invoker.SetCommand(command);
            invoker.ExecuteCommand();
        }
    }
}