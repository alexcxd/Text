using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SampleCode.DesignPattern.StructuralPatterns
{
    public class CompositePattern
    {
        /// <summary>
        /// 组合模式
        /// </summary>
        /// 就是棵树
        public static void CompositePatternMain()
        {
            CompositePattern2();
        }


        /// <summary>
        /// 透明式
        /// </summary>
        public static void CompositePattern1()
        {
            var root = new Compasite("root");
            root.Add(new Leaf("Leaf RA"));
            root.Add(new Leaf("Leaf RB"));

            var compasiteA = new Compasite("compasiteA");
            compasiteA.Add(new Leaf("Leaf CA"));
            compasiteA.Add(new Leaf("Leaf CB"));
            root.Add(compasiteA);

            var compasiteB = new Compasite("compasiteB");
            compasiteB.Add(new Leaf("Leaf CA"));
            compasiteB.Add(new Leaf("Leaf CB"));
            root.Add(compasiteB);

            //list.Remove(Object)是通过比较引用地址
            //因此新new出的无法将List中的数据移除

            var leafC = new Leaf("Leaf RC");
            root.Add(leafC);
            root.Operation(1);
            var memory1 = GetMemory(leafC);

            var leafC1 = new Leaf("Leaf RC");
            root.Remove(leafC1);
            var memory2 = GetMemory(leafC1);

            root.Remove(leafC);
            root.Operation(1);
        }

        /// <summary>
        /// 安全式
        /// </summary>
        public static void CompositePattern2()
        {
            var root = new ConcreteCompasiteSafe("root");
            root.Add(new ConcreteLeafSafe("Leaf RA"));
            root.Add(new ConcreteLeafSafe("Leaf RB"));

            var compasiteA = new ConcreteCompasiteSafe("compasiteA");
            compasiteA.Add(new ConcreteLeafSafe("Leaf CA"));
            compasiteA.Add(new ConcreteLeafSafe("Leaf CB"));
            root.Add(compasiteA);

            var compasiteB = new ConcreteCompasiteSafe("compasiteB");
            compasiteB.Add(new ConcreteLeafSafe("Leaf CA"));
            compasiteB.Add(new ConcreteLeafSafe("Leaf CB"));
            root.Add(compasiteB);

            //list.Remove(Object)是通过比较引用地址
            //因此新new出的无法将List中的数据移除

            var leafC = new ConcreteCompasiteSafe("Leaf RC");
            root.Add(leafC);
            root.Operation(1);
           
            root.Remove(leafC);
            root.Operation(1);
        }

        /// <summary>
        /// 获取引用类型的内存地址方法    
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string GetMemory(object o) 
        {
            GCHandle h = GCHandle.Alloc(o, GCHandleType.WeakTrackResurrection);

            IntPtr addr = GCHandle.ToIntPtr(h);

            return "0x" + addr.ToString("X");
        }
    }


    #region 透明式
    // 为了消除叶节点和枝节点在抽象层次的区别，即维持透明

    /// <summary>
    /// 抽象构件角色
    /// </summary>
    public abstract class Component
    {
        protected string Name { get; set; }

        public Component(string name)
        {
            this.Name = name;
        }

        public abstract void Add(Component component);

        public abstract void Remove(Component component);

        public abstract void Operation(int depth);
    }

    /// <summary>
    /// 枝节点
    /// </summary>
    public class Compasite : Component
    {
        private List<Component> components = new List<Component>();

        public Compasite(string name): base(name) { }

        public override void Add(Component component)
        {
            components.Add(component);
        }

        public override void Operation(int depth)
        {
            Console.WriteLine($"枝节点名：{new String('-', depth)}{Name}");
            //遍历下级节点
            depth += 2;
            foreach (var component in components)
            {
                component.Operation(depth);
            }
        }

        public override void Remove(Component component)
        {
            components.Remove(component);
        }
    }

    /// <summary>
    /// 叶节点
    /// </summary>
    public class Leaf : Component
    {
        public Leaf(string name) : base(name)
        {

        }

        public override void Operation(int depth)
        {
            Console.WriteLine($"叶节点名：{new String('-', depth)}{Name}");
        }

        public override void Add(Component component)
        {

        }
        public override void Remove(Component component)
        {

        }
    }
    #endregion

    #region 安全式
    //可以有效的区分枝叶节点，且责任很明确 

    /// <summary>
    /// 抽象构件角色
    /// </summary>
    public abstract class ComponentSafe
    {
        protected string Name { get; set; }

        public ComponentSafe(string name)
        {
            this.Name = name;
        }


        public abstract void Operation(int depth);
    }

    /// <summary>
    /// 抽象枝节点
    /// </summary>
    public abstract class CompasiteSafe : ComponentSafe
    {
        protected List<ComponentSafe> components = new List<ComponentSafe>();

        public CompasiteSafe(string name) : base(name) { }

        public abstract void Add(ComponentSafe component);

        public abstract void Remove(ComponentSafe component);

        public override void Operation(int depth)
        {
            Console.WriteLine($"枝节点名：{new String('-', depth)}{Name}");
            //遍历下级节点
            depth += 2;
            foreach (var component in components)
            {
                component.Operation(depth);
            }
        }
    }

    /// <summary>
    /// 具体枝节点
    /// </summary>
    public class ConcreteCompasiteSafe : CompasiteSafe
    {
        public ConcreteCompasiteSafe(string name) : base(name) { }

        public override void Add(ComponentSafe component)
        {
            components.Add(component);
        }

        public override void Remove(ComponentSafe component)
        {
            components.Remove(component);
        }
    }

    /// <summary>
    /// 具体叶节点
    /// </summary>
    public class ConcreteLeafSafe : ComponentSafe
    {
        public ConcreteLeafSafe(string name) : base(name)
        {

        }

        public override void Operation(int depth)
        {
            Console.WriteLine($"叶节点名：{new String('-', depth)}{Name}");
        }
    }
    #endregion
}