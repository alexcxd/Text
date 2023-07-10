using System;

namespace SampleCode.DesignPattern.CreationalPatterns.Factory
{
    public class SimpleFactoryPattern
    {
        /// <summary>
        /// 简单工厂模式
        /// </summary>
        public static void SimpleFactoryPatternMain()
        {
            var oper = OperationFactory.CreateOperation("-");
            oper.NumbleA = 1;
            oper.NumbleB = 3;
            Console.WriteLine("结果为：" + oper.GetResult());
            Console.ReadKey();
        }
       
    }

    public class OperationFactory
    {
        public static Operation CreateOperation(string operation)
        {
            Operation oper = null;

            switch (operation)
            {
                case "+":
                    oper = new OperationAdd();
                    break;
                case "-":
                    oper = new OperationSub();
                    break;
                case "*":
                    oper = new OperationMul();
                    break;
                case "/":
                    oper = new OperationDiv();
                    break;
            }

            return oper;

        }
    }

    public class Operation
    {
        public double NumbleA { get; set; }

        public double NumbleB { get; set; }

        protected double result = 0;

        public virtual double GetResult()
        {
            return result;
        }
    }

    public class OperationAdd : Operation
    {
        public override double GetResult()
        {
            result = NumbleA + NumbleB;
            return result;
        }
    }

    public class OperationSub : Operation
    {
        public override double GetResult()
        {
            result = NumbleA - NumbleB;
            return result;
        }
    }

    public class OperationMul : Operation
    {
        public override double GetResult()
        {
            result = NumbleA * NumbleB;
            return result;
        }
    }

    public class OperationDiv : Operation
    {
        public override double GetResult()
        {
            if(NumbleB == 0)
            {
                throw new Exception("除数不可为0");
            }
            result = NumbleA / NumbleB;
            return result;
        }
    }

}
