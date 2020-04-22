using NUnit.Framework;
using SampleCode.DesignPattern.行为型模式;

namespace SampleCode.Test.DesignPattern.行为型模式
{
    [TestFixture]
    public class MediatorPatternTest
    {
        /// <summary>
        /// 中介者模式
        /// </summary>
        [Test]
        public void MediatorPatternCodeTest()
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
}