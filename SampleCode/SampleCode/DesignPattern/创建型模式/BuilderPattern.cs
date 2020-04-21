using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DesignPattern
{
    /// <summary>
    /// 建造者模式
    /// </summary>
    /// 将一个复杂对象的构建与它的表示分离,使的同样的构建过程可以创建不同的表示
    public class BuilderPattern
    {
        public static void BuliderPatternMain()
        {

            ConcreteBuilderA concreteBuilderA = new ConcreteBuilderA();
            ConcreteBuilderB concreteBuilderB = new ConcreteBuilderB();

            Director director = new Director();
            director.Construct(concreteBuilderA);
            var aProcudt = concreteBuilderA.GetResult();
            aProcudt.Show();

            director.Construct(concreteBuilderB);
            var bProduct = concreteBuilderB.GetResult();
            bProduct.Show();

        }
    }

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
            foreach(var part in parts)
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
        public void Construct(Builder builder)
        {
            builder.BuilderPartA();
            builder.BuilderPartB();
        }
    }
}
