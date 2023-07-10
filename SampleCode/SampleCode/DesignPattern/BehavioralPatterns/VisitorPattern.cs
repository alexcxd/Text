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
    /// ???未理解透
    public class VisitorPattern
    {
        public static void VisitorPatternMain()
        {
            ConcreteElementA elementA = new ConcreteElementA();
            ConcreteElementB elementB = new ConcreteElementB();
            ObjectStruture struture = new ObjectStruture();
            struture.Add(elementA);
            struture.Add(elementB);

            ConcreteVisitor1 v1 = new ConcreteVisitor1();
            struture.Aceppt(v1);
        }
    }

    public abstract class Visitor
    {
        public abstract void VisitorConcreteElementA(ConcreteElementA element);
        public abstract void VisitorConcreteElementB(ConcreteElementB element);
    }

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

    public abstract class Element
    {
        public abstract void Accept(Visitor visitor);
    }

    public class ConcreteElementA : Element
    {
        public override void Accept(Visitor visitor)
        {
            visitor.VisitorConcreteElementA(this);
        }
    }
    
    public class ConcreteElementB : Element
    {
        public override void Accept(Visitor visitor)
        {
            visitor.VisitorConcreteElementB(this);
        }
    }

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
