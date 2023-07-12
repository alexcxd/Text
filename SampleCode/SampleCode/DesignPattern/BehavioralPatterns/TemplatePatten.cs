namespace SampleCode.DesignPattern.BehavioralPatterns
{
    /// <summary>
    /// 模板模式
    /// 定义一个操作中的算法骨架，而将一些步骤延迟到子类中。模板方法使得子类可以不改变一个算法的结构即可重定义该算法的
    /// </summary>
    public class TemplatePatten
    {
        public static void TemplatePattenMain()
        {
            AbstractClass concreteClass1 = new ConcreteClass1();
            concreteClass1.TemplateMethod();

            AbstractClass concreteClass2 = new ConcreteClass2();
            concreteClass2.TemplateMethod();
        }

        /// <summary>
        /// 抽象模板方法
        /// </summary>
        public abstract class AbstractClass
        {
            //基本方法
            protected abstract void DoSomething();

            //基本方法
            protected abstract void DoAnything();

            ////模板方法
            public void TemplateMethod()
            {
                //调用基本方法，完成相关的逻辑
                this.DoAnything();
                this.DoSomething();

            }
        }

        /// <summary>
        /// 具体模板类
        /// </summary>
        public class ConcreteClass1 : AbstractClass
        {
            protected override void DoSomething()
            {
                // 业务逻辑处理
            }

            protected override void DoAnything()
            {
                // 业务逻辑处理
            }
        }

        /// <summary>
        /// 具体模板类
        /// </summary>
        public class ConcreteClass2 : AbstractClass
        {
            protected override void DoSomething()
            {
                // 业务逻辑处理
            }

            protected override void DoAnything()
            {
                // 业务逻辑处理
            }
        }
    }
}