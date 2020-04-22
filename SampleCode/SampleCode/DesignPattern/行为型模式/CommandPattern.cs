using System;

namespace SampleCode.DesignPattern.行为型模式
{
    /// <summary>
    /// 命令模式
    /// </summary>
    /// 将一个请求封装成一个对象，从而使你可用不同的请求对客户进行参数化；
    /// 对请求排队或记录请求日志，以及支持可撤销功能
    /// 适用：
    /// 1. 命令的发送者和命令执行者有不同的生命周期。命令发送了并不是立即执行。
    /// 2. 命令需要进行各种管理逻辑。
    /// 3. 需要支持撤消\重做操作（这种状况的代码大家可以上网搜索下，有很多，这里不进行详细解读）。

    public class CommandPattern
    {
        public static void CommandPatternMain()
        {
            Receiver receiver = new Receiver();
            ConcreteCommand command = new ConcreteCommand();
            command.SetReceiver(receiver);
            Invoker invoker = new Invoker();
            invoker.SetCommand(command);
            invoker.ExecuteCommand();

        }
    }

    /// <summary>
    /// 抽象命令类
    /// </summary>
    public abstract class Command
    {
        protected Receiver receiver;

        public void SetReceiver(Receiver receiver)
        {
            this.receiver = receiver;
        }

        abstract public void Execute();
    }

    /// <summary>
    /// 具体命令类
    /// </summary>
    public class ConcreteCommand : Command
    {
        public override void Execute()
        {
            receiver.Action();
        }
    }

    /// <summary>
    /// 命令实现类
    /// </summary>
    public class Receiver
    {
        public void Action()
        {
            Console.WriteLine("执行请求");
        }
    }

    /// <summary>
    /// 命令请求类
    /// </summary>
    public class Invoker
    {
        private Command command;

        public void SetCommand(Command command)
        {
            this.command = command;
        }

        public void ExecuteCommand()
        {
            command.Execute();
        }

    }
}
