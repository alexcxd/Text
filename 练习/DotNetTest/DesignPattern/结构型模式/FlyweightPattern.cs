using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTest.DesignPattern
{
    /// <summary>
    /// 享元模式
    /// </summary>
    /// 享元模式的目的就是使用共享技术来实现大量细粒度对象的复用
    /// 享元模式的本质是分离与共享 ： 分离变与不变，并且共享不变
    /// 在享元模式中通常会出现工厂模式，
    /// 需要创建一个享元工厂来负责维护一个享元池(Flyweight Pool)（用于存储具有相同内部状态的享元对象）。
    class FlyweightPattern
    {
        public static void FlyweightPatternMain()
        {
            FlyweightFactory factory = new FlyweightFactory();

            var flyweightA1 = factory.GetFlyweight("A");
            flyweightA1.Operation();

            var flyweightA2 = factory.GetFlyweight("A");
        }
    }

    /// <summary>
    /// 抽象享元角色类
    /// </summary>
    public interface IFlyweight
    {
        void Operation();
    }

    /// <summary>
    /// 具体享元角色类
    /// </summary>
    public class ConcreteFlyweight : IFlyweight
    {
        private string state;
        public ConcreteFlyweight(string state)
        {
            this.state = state;
        }

        public void Operation()
        {
            Console.WriteLine("具体的Flyweight" + state);
        }
    }

    /// <summary>
    /// 享元工厂
    /// </summary>
    /// 可与反射，组合模式搭配使用
    public class FlyweightFactory
    {
        Dictionary<string, IFlyweight> flyweights = new Dictionary<string, IFlyweight>();

        public IFlyweight GetFlyweight(string state)
        {
            ConcreteFlyweight flyweight;

            if (!flyweights.ContainsKey(state))
            {
                flyweight = new ConcreteFlyweight(state);
                flyweights.Add(state, flyweight);
            }
            else
            {
                flyweight = flyweights[state] as ConcreteFlyweight;
            }
            return flyweight;
        }
    }

}
