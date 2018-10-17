using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DesignPattern
{
    /// <summary>
    /// 装饰器模式1
    /// </summary
    /// 动态的给一个对象添加额外的职责，就增加功能来说，它比子类更灵活

    class DecoratorPattern
    {
        public static void DecoratorPatternMain()
        {
            // 在原来的基础上增加功能
            Console.WriteLine("在原来的基础上增加功能");
            ConcreteComponent concreteComponent = new ConcreteComponent();

            DecoratorA decoratorA = new DecoratorA(concreteComponent);
            DecoratorB decoratorB = new DecoratorB(decoratorA);

            decoratorB.Operation();

            Console.WriteLine("-------------------------------------");

            // 覆盖掉原来的功能
            Console.WriteLine("覆盖掉原来的功能");
            CurrComponent currComponent = new CurrComponent();
            currComponent.Operation();
        }
    }

    /// <summary>
    /// 对象接口
    /// </summary>
    public abstract class Component
    {
        public abstract void Operation();
    }

    /// <summary>
    /// 需要添加功能的对象
    /// </summary>
    public class ConcreteComponent : Component
    {
        public override void Operation()
        {
            Console.WriteLine("某对象功能");
        }
    }

    /// <summary>
    /// 装饰器抽象类
    /// </summary>
    public abstract class Decorator : Component
    {
        private Component super;

        public Decorator(Component component)
        {
            super = component;
        }

        public override void Operation()
        {
            if(super != null)
            {
                super.Operation();
            }
        }

    }

    /// <summary>
    /// 装饰功能A
    /// </summary>
    public class DecoratorA : Decorator
    {
        public DecoratorA(Component component) : base(component) { }
        
        public override void Operation()
        {
            base.Operation();
            Console.WriteLine("装饰功能A");
        }
    }

    /// <summary>
    /// 装饰功能B
    /// </summary>
    public class DecoratorB : Decorator
    {
        public DecoratorB(Component component) : base(component) { }

        public override void Operation()
        {
            base.Operation();
            Console.WriteLine("装饰功能B");
        }
    }


    public class CurrComponent : ConcreteComponent
    {
        public override void Operation()
        {
            Console.WriteLine("覆盖功能");
        }
    }
}
