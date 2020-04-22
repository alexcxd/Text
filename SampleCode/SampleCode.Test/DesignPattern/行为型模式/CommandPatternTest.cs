using NUnit.Framework;
using SampleCode.DesignPattern.行为型模式;

namespace SampleCode.Test.DesignPattern.行为型模式
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