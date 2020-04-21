using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DesignPattern
{
    /// <summary>
    /// 策略模式
    /// </summary>
    /// 定义算法家族，分别封装起来，让它们之间可以互相替换
    /// 应用场景
    /// 1、 多个类只区别在表现行为不同，可以使用Strategy模式，在运行时动态选择具体要执行的行为。
    /// 2、 需要在不同情况下使用不同的策略(算法)，或者策略还可能在未来用其它方式来实现。
    /// 3、 对客户隐藏具体策略(算法)的实现细节，彼此完全独立。
    /// 举例：反射中MemberInfo（抽象算法类）和其他(Methodinfo等)（具体的算法）
    public class StrategyPattern
    {
        public static void StrategyPatternMain()
        {
            Context context;

            context = new Context(new ConcreteStrategyA());
            context.ContextInterface();

            context = new Context(new ConcreteStrategyB());
            context.ContextInterface();

        }
    }

    //也可以用接口
    public abstract class Strategy
    {
        public abstract void AlgorithmInterface();
    }


    public class ConcreteStrategyA : Strategy
    {
        public override void AlgorithmInterface()
        {
            Console.WriteLine("实现了策略A");
        }
    }

    public class ConcreteStrategyB : Strategy
    {
        public override void AlgorithmInterface()
        {
            Console.WriteLine("实现了策略B");
        }
    }

    public class Context
    {

        private Strategy strategy;

        public Context(Strategy strategy)
        {
            this.strategy = strategy;
        }

        public void ContextInterface()
        {
            if(strategy == null)
            {
                throw new Exception("对象为空");
            }
            strategy.AlgorithmInterface();
        }
    }

}
