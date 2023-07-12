using System;
using System.Collections.Generic;

namespace SampleCode.DesignPattern.CreationalPatterns
{
    /// <summary>
    /// 建造者模式
    /// 将一个复杂对象的构建与它的表示分离,使的同样的构建过程可以创建不同的表示
    /// </summary>
    public class BuilderPattern
    {
        public static void BuilderPatternMain()
        {
            var concreteBuilderA = new ConcreteBuilderA();
            var concreteBuilderB = new ConcreteBuilderB();

            var director = new Director();
            var aProduct = director.GetProduct(concreteBuilderA);
            aProduct.Show();

            var bProduct = director.GetProduct(concreteBuilderB);
            bProduct.Show();

        }

        /// <summary>
        /// 产品类
        /// </summary>
        public class Product
        {
            private List<string> parts = new List<string>();
            public void Add(string part)
            {
                parts.Add(part);
            }
            public void Show()
            {
                Console.WriteLine("产品创建");
                foreach (var part in parts)
                {
                    Console.WriteLine(part);
                }
            }
        }

        /// <summary>
        /// 建造者抽象类
        /// </summary>
        public abstract class Builder
        {
            public abstract void BuilderPartA();
            public abstract void BuilderPartB();
            public abstract Product GetResult();
        }

        /// <summary>
        /// 具体建造类
        /// </summary>
        public class ConcreteBuilderA : Builder
        {
            private Product product = new Product();

            public override void BuilderPartA()
            {
                product.Add("部件A");
            }

            public override void BuilderPartB()
            {
                product.Add("部件B");
            }

            public override Product GetResult()
            {
                return product;
            }
        }

        /// <summary>
        /// 具体建造类
        /// </summary>
        public class ConcreteBuilderB : Builder
        {
            private Product product = new Product();

            public override void BuilderPartA()
            {
                product.Add("部件X");
            }

            public override void BuilderPartB()
            {
                product.Add("部件Y");
            }

            public override Product GetResult()
            {
                return product;
            }
        }

        /// <summary>
        /// 指挥者类
        /// </summary>
        /// <remarks>用来指挥建造过程</remarks>
        public class Director
        {
            public Product GetProduct(Builder builder)
            {
                builder.BuilderPartA();
                builder.BuilderPartB();

                return builder.GetResult();
            }
        }
    }

}
