﻿using System;

namespace SampleCode.DesignPattern.创建型模式.Factory
{
    public class FactoryPattern
    {
        /// <summary>
        /// 工厂模式
        /// </summary>
        public static void FactoryPatternMain()
        {
            var factory = new AddFactory();
            var opereation = factory.CreateOpereation();
            opereation.NumbleA = 100;
            opereation.NumbleB = 10.1;
            var result = opereation.GetResult();
            Console.WriteLine(result);
        }

    }

    public interface IFrctory
    {
        Operation CreateOpereation();
    }


    public class AddFactory : IFrctory
    {
        public Operation CreateOpereation()
        {
            return new OperationAdd();
        }
    }

    public class SubFactory : IFrctory
    {
        public Operation CreateOpereation()
        {
            return new OperationSub();
        }
    }

    public class MulFactory : IFrctory
    {
        public Operation CreateOpereation()
        {
            return new OperationMul();
        }
    }

    public class DivFactory : IFrctory
    {
        public Operation CreateOpereation()
        {
            return new OperationDiv();
        }
    }
}
