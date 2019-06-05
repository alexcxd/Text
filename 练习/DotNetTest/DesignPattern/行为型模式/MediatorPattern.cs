using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTest.DesignPattern
{
    /// <summary>
    /// 中介者模式
    /// </summary>
    /// 用一个中介对象来封装一系列的对象交互。
    /// 中介者使个对象不用显式的相互引用，从而使得松耦合
    class MediatorPattern
    {
        public static void MediatorPatternMain()
        {
            ConcreteMediator mediator = new ConcreteMediator();

            ConcreteColleagueA colleagueA = new ConcreteColleagueA(1, mediator);

            ConcreteColleagueB colleagueB = new ConcreteColleagueB(2, mediator);

            mediator.AddColleague(colleagueA);
            mediator.AddColleague(colleagueB);

            colleagueA.Send("你好", colleagueB);
            colleagueB.Send("hi", colleagueA);
        }
    }

    /// <summary>
    /// 抽象中介类
    /// </summary>
    public abstract class Mediator
    {
        public abstract void Send(String message, Colleague colleagueTarget, Colleague colleague);
    }

    /// <summary>
    /// 抽象同事类
    /// </summary>
    public abstract class Colleague
    {
        protected Mediator mediator;
        public int Id { get; set; }

        public Colleague(int id, Mediator mediator)
        {
            this.mediator = mediator;
            this.Id = id;
        }

        public abstract void Notify(String message, Colleague colleague);

        public abstract void Send(String message, Colleague colleagueTarget);
    }

    /// <summary>
    /// 具体中介类
    /// </summary>
    public class ConcreteMediator : Mediator
    {
        private List<Colleague> colleagues = new List<Colleague>();

        public void AddColleague(Colleague colleague)
        {
            colleagues.Add(colleague);
        }

        public void RemoveColleague(Colleague colleague)
        {
            colleagues.Remove(colleague);
        }

        public override void Send(string message, Colleague colleagueTarget, Colleague colleague)
        {
            if (colleagues.Exists(m => m.Id == colleagueTarget.Id))
            {
                colleague.Notify(message, colleagueTarget);
            }
        }
    }

    /// <summary>
    /// 不同功能的同事类
    /// </summary>
    public class ConcreteColleagueA : Colleague
    {
        public ConcreteColleagueA(int id, Mediator mediator) : base(id, mediator)
        {
        }

        public override void Notify(string message, Colleague colleague)
        {
            Console.WriteLine($"获取来自{colleague.Id}的消息  {message}");
        }

        public override void Send(string message, Colleague colleagueTarget)
        {
            mediator.Send(message, colleagueTarget, this);
        }
    }

    public class ConcreteColleagueB : Colleague
    {
        public ConcreteColleagueB(int id, Mediator mediator) : base(id, mediator)
        {
        }

        public override void Notify(string message, Colleague colleague)
        {
            Console.WriteLine($"获取来自{colleague.Id}的消息  {message}");
        }

        public override void Send(string message, Colleague colleagueTarget)
        {
            mediator.Send(message, colleagueTarget, this);
        }
    }
}
