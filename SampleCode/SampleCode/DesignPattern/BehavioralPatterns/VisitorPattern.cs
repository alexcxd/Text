using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode.DesignPattern.BehavioralPatterns
{
    /// <summary>
    /// 访问者模式
    /// </summary>
    public class VisitorPattern
    {
        public static void VisitorPatternMain()
        {
            var elementA = new ConcreteElementA();
            var elementB = new ConcreteElementB();
            var struture = new ObjectStruture();
            struture.Add(elementA);
            struture.Add(elementB);

            var v1 = new ConcreteVisitor1();
            struture.Aceppt(v1);
        }
    }

    /// <summary>
    /// 抽象访问者
    /// 一般是有几个具体元素就有几个访问方法。
    /// </summary>
    public abstract class Visitor
    {
        public abstract void VisitorConcreteElementA(ConcreteElementA element);
        public abstract void VisitorConcreteElementB(ConcreteElementB element);
    }

    /// <summary>
    /// 具体访问者1
    /// </summary>
    public class ConcreteVisitor1 : Visitor
    {
        public override void VisitorConcreteElementA(ConcreteElementA element)
        {
            Console.WriteLine($"{element.GetType().Name}被{this.GetType().Name}访问");
        }

        public override void VisitorConcreteElementB(ConcreteElementB element)
        {
            Console.WriteLine($"{element.GetType().Name}被{this.GetType().Name}访问");
        }
    }

    /// <summary>
    /// 具体访问者2
    /// </summary>
    public class ConcreteVisitor2 : Visitor
    {
        public override void VisitorConcreteElementA(ConcreteElementA element)
        {
            Console.WriteLine($"{element.GetType().Name}被{this.GetType().Name}访问");
        }

        public override void VisitorConcreteElementB(ConcreteElementB element)
        {
            Console.WriteLine($"{element.GetType().Name}被{this.GetType().Name}访问");
        }
    }

    /// <summary>
    /// 抽象元素
    /// </summary>
    public abstract class Element
    {
        /// <summary>
        /// 允许谁来访问
        /// </summary>
        /// <param name="visitor"></param>
        public abstract void Accept(Visitor visitor);
    }

    /// <summary>
    /// 具体元素A
    /// </summary>
    public class ConcreteElementA : Element
    {
        public override void Accept(Visitor visitor)
        {
            visitor.VisitorConcreteElementA(this);
        }
    }

    /// <summary>
    /// 具体元素B
    /// </summary>
    public class ConcreteElementB : Element
    {
        public override void Accept(Visitor visitor)
        {
            visitor.VisitorConcreteElementB(this);
        }
    }

    /// <summary>
    /// 元素产生者
    /// 一般容纳在多个不同类、不同接口的容器，如List、Set、Map等，在项目中，一般很少抽象出这个角色。
    /// </summary>
    public class ObjectStruture
    {
        private List<Element> elements = new List<Element>();

        public void Add(Element element)
        {
            elements.Add(element);
        }

        public void Remove(Element element)
        {
            elements.Remove(element);
        }

        public void Aceppt(Visitor visitor)
        {
            foreach(var element in elements)
            {
                element.Accept(visitor);
            }
        }
    }
}
