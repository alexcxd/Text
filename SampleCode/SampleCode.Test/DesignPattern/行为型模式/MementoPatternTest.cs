using NUnit.Framework;
using SampleCode.DesignPattern.BehavioralPatterns;

namespace SampleCode.Test.DesignPattern.BehavioralPatterns
{
    [TestFixture]
    public class MementoPatternTest
    {
        /// <summary>
        /// 备忘录模式
        /// </summary>
        [Test]
        public void MementoPatternCodeTest()
        {
            //MyMemento();
            BookMemento();
        }

        /// <summary>
        /// 自己写的
        /// </summary>
        /// 通过Originator的构造函数是否给MementoInfo对象来判断是否备忘
        /// 没有实现Originator和MementoInfo的分离,且备份不够灵活
        public static void MyMemento()
        {
            MyOriginator originator = new MyOriginator(new MyMementoInfo());
            originator.State = "1";
            originator.State = "2";
            originator.State = "3";
            originator.State = "4";
            originator.State = "5";

            var MementoInfo = originator.GetMementoInfo();
            originator.RollBack();
        }

        public static void BookMemento()
        {
            BookOriginator originator = new BookOriginator();
            originator.State = "1";
            var mementoInfo = new BookMementoInfo();
            mementoInfo.memento = originator.CreateMemento();

            originator.State = "2";
            originator.Show();

            originator.SetMemento(mementoInfo.memento);
            originator.Show();

        }
    }
}