using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTest.DesignPattern
{
    /// <summary>
    /// 备忘录模式
    /// </summary>
    /// 在不破坏封装的前提下，捕获一个对象的内部状态，并在改对象之外保存这个状态（未实现）
    public class MementoPattern
    {
        public static void MementoPatternMain()
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

    #region 书部分
    /// <summary>
    /// 发起人
    /// </summary>
    public class BookOriginator
    {
        public BookOriginator() { }

        public string State { get; set; }

        /// <summary>
        /// 恢复备忘录
        /// </summary>
        public void SetMemento(BookMemento memento)
        {
            this.State = memento.State;
        }

        /// <summary>
        /// 创造备忘录
        /// </summary>
        /// <returns></returns>
        public BookMemento CreateMemento()
        {
            return new BookMemento(State);
        }

        public void Show()
        {
            Console.WriteLine($"当前状态为：{State}");
        }
    }

    /// <summary>
    /// 备忘录
    /// </summary>
    public class BookMemento
    {
        private string state;

        public BookMemento(string state)
        {
            this.state = state;
        }

        public string State { get { return state; } }
    }

    /// <summary>
    /// 备忘录管理类
    /// </summary>
    public class BookMementoInfo
    {
        public BookMemento memento { get; set; }
    }
    #endregion

    #region 自己部分
    /// <summary>
    /// 发起人
    /// </summary>
    public class MyOriginator
    {
        public MyOriginator() { }
        public MyOriginator(MyMementoInfo mementoInfo)
        {
            this.mementoInfo = mementoInfo;
        }

        private string state;
        private MyMementoInfo mementoInfo;
        public string State
        {
            get{ return state; }
            set
            {
                if (mementoInfo != null && state != null)
                {
                    mementoInfo.memento.SetState(state);
                }
                state = value;
                
            }
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public void RollBack()
        {
            if (mementoInfo != null)
            {
                this.state = mementoInfo.memento.GetState();
            }
                
        }

        public MyMementoInfo GetMementoInfo()
        {
            return mementoInfo;
        }

    }

    /// <summary>
    /// 备忘录
    /// </summary>
    public class MyMemento
    {
        private Dictionary<string, DateTime> StateDictionary;

        public MyMemento()
        {
            StateDictionary = new Dictionary<string, DateTime>();
        }

        public void SetState(string state)
        {
            StateDictionary.Add(state, DateTime.Now);
        }

        public string GetState()
        {
            var state = StateDictionary.LastOrDefault().Key;
            StateDictionary.Remove(state);
            return state;
        }

        public Dictionary<string, DateTime> GetStates()
        {
            return StateDictionary;
        }
    }

    /// <summary>
    /// 备忘录管理类
    /// </summary>
    public class MyMementoInfo
    {
        public MyMementoInfo()
        {
            memento = new MyMemento();
        }
        public MyMemento memento { get; }
    }
    #endregion
}
