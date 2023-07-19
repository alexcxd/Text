using System;
using System.Collections.Generic;

namespace SampleCode.DesignPattern.BehavioralPatterns
{
    /// <summary>
    /// 观察者模式
    /// </summary>
    /// 定义了一种一对多的依赖关系，让多个观察者对象同时监听某一个主题对象。
    /// 这个主题对象在状态发生变化时，会通知所有观察者对象，使它们能够自动更新自己。
    /// 举例：监听器，Form中各控件间的联系
    public class ObserverPattern
    {
        public static void ObserverPatternMain()
        {
            Subject subject = new ConcreteSubject();
            subject.Attach(new ConcreteObserver("订阅者A"));
            subject.Attach(new ConcreteObserver("订阅者B"));
            subject.Attach(new ConcreteObserver("订阅者C"));

            subject.Info = "某种变化";
            subject.Notify();
        }
    }

    /// <summary>
    /// 被观察者抽象类
    /// </summary>
    public abstract class Subject
    {
        private List<IObserver> observers = new List<IObserver>();
        public string Info { get; set; }

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Remove(IObserver observer)
        {
            observers.Remove(observer);
        }

        //通知
        public void Notify()
        {
            foreach(var observer in observers)
            {
                observer.Update(this);
            }
        }
    }

    /// <summary>
    /// 具体被观察者
    /// </summary>
    public class ConcreteSubject : Subject
    {

    }

    /// <summary>
    /// 观察者接口
    /// </summary>
    public  interface IObserver
    {
        void Update(Subject subject);
    }

    /// <summary>
    /// 具体观察者
    /// </summary>
    public class ConcreteObserver : IObserver
    {
        public string Name { get; set; }

        public ConcreteObserver(string name)
        {
            this.Name = name;
        }

        public void Update(Subject subject)
        {
            Console.WriteLine($"{Name}观察到了 {subject.Info}");
        }
    }
}
