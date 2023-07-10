using System;

namespace SampleCode.DesignPattern.StructuralPatterns
{
    /// <summary>
    /// 装饰器模式1
    ///  动态的给一个对象添加额外的职责，就增加功能来说，它比子类更灵活
    /// </summary>

    public class DecoratorPattern
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
    public abstract class DecoratorComponent
    {
        public abstract void Operation();
    }

    /// <summary>
    /// 需要添加功能的对象
    /// </summary>
    public class ConcreteComponent : DecoratorComponent
    {
        public override void Operation()
        {
            Console.WriteLine("某对象功能");
        }
    }

    /// <summary>
    /// 装饰器抽象类
    /// </summary>
    public abstract class Decorator : DecoratorComponent
    {
        private DecoratorComponent super;

        public Decorator(DecoratorComponent component)
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
        public DecoratorA(DecoratorComponent component) : base(component) { }
        
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
        public DecoratorB(DecoratorComponent component) : base(component) { }

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
