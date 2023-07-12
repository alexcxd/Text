using System;

namespace SampleCode.DesignPattern.CreationalPatterns.Factory
{
    /// <summary>
    /// 工厂方法模式
    /// 在某个类中定义用来提供所需服务对象的方法
    /// </summary>
    public class FactoryPattern
    {
        public static void FactoryPatternMain()
        {
            var factory = new AddFactory();
            var operation = factory.CreateOperation();
            operation.NumberA = 100;
            operation.NumberB = 10.1;
            var result = operation.GetResult();
            Console.WriteLine(result);
        }

    }

    public partial interface IFactory
    {
        Operation CreateOperation();
    }


    public class AddFactory : IFactory
    {
        public Operation CreateOperation()
        {
            return new OperationAdd();
        }
    }

    public class SubFactory : IFactory
    {
        public Operation CreateOperation()
        {
            return new OperationSub();
        }
    }

    public class MulFactory : IFactory
    {
        public Operation CreateOperation()
        {
            return new OperationMul();
        }
    }

    public class DivFactory : IFactory
    {
        public Operation CreateOperation()
        {
            return new OperationDiv();
        }
    }
}
