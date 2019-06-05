using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTest.DesignPattern
{
    class BridgePattern
    {
        /// <summary>
        /// 桥接模式
        /// </summary>
        /// 将抽象部分与它的实现部分分离
        /// 即实现系统可能出现的多角度分类，每一种分类都可能变化，那么就把这种多角度分离出来让他们独立变化，减少他们的耦合
        public static void BridgePatternMian()
        {
            RefinedAbstranction abstranction = new RefinedAbstranction();
            abstranction.SetImplementor(new ConreteImplementorA());
            abstranction.Operation();

            abstranction.SetImplementor(new ConreteImplementorB());
            abstranction.Operation();
        }
    }


    /// <summary>
    /// 实现抽象类
    /// </summary>
    public abstract class Implementor
    {
        public abstract void Operator();
    }

    /// <summary>
    /// 实现具体类A
    /// </summary>
    public class ConreteImplementorA : Implementor
    {
        public override void Operator()
        {
            Console.WriteLine("实现A");
        }
    }

    /// <summary>
    /// 实现具体类B
    /// </summary>
    public class ConreteImplementorB : Implementor
    {
        public override void Operator()
        {
            Console.WriteLine("实现B");
        }
    }

    /// <summary>
    /// 实现虚类(抽象)
    /// </summary>
    public class Abstranction
    {
        protected Implementor Implementor;

        public void SetImplementor(Implementor implementor)
        {
            this.Implementor = implementor;
        }

        public virtual void Operation()
        {
            Implementor.Operator();
        }
    }

    /// <summary>
    /// 被提炼的抽象
    /// </summary>
    public class RefinedAbstranction : Abstranction
    {
        public override void Operation()
        {
            Implementor.Operator();
        }
    }
}
