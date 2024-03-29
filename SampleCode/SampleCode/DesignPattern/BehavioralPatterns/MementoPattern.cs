﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleCode.DesignPattern.BehavioralPatterns
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
            BookMementoTest();
        }

        /// <summary>
        /// 自己写的
        /// </summary>
        /// 通过Originator的构造函数是否给MementoInfo对象来判断是否备忘
        /// 没有实现Originator和MementoInfo的分离,且备份不够灵活
        public static void MyMementoTest()
        {
            MyOriginator originator = new MyOriginator(new MyMementoManager());
            originator.State = "1";
            originator.State = "2";
            originator.State = "3";
            originator.State = "4";
            originator.State = "5";

            var MementoInfo = originator.GetMementoInfo();
            originator.RollBack();
        }

        public static void BookMementoTest()
        {
            BookOriginator originator = new BookOriginator();
            originator.State = "1";
            var mementoInfo = new BookMementoManager();
            mementoInfo.memento = originator.CreateMemento();

            originator.State = "2";
            originator.Show();

            originator.SetMemento(mementoInfo.memento);
            originator.Show();

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
        public class BookMementoManager
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
            public MyOriginator(MyMementoManager mementoInfo)
            {
                this.mementoInfo = mementoInfo;
            }

            private string state;
            private MyMementoManager mementoInfo;
            public string State
            {
                get => state;
                set
                {
                    if (mementoInfo != null && state != null)
                    {
                        mementoInfo.Memento.SetState(state);
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
                    this.state = mementoInfo.Memento.GetState();
                }

            }

            public MyMementoManager GetMementoInfo()
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
        public class MyMementoManager
        {
            public MyMementoManager()
            {
                Memento = new MyMemento();
            }
            public MyMemento Memento { get; }
        }
        #endregion
    }

}
