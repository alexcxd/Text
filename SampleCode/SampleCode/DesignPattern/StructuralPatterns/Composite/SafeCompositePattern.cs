using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SampleCode.DesignPattern.StructuralPatterns
{
    /// <summary>
    /// 组合模式 - 安全式
    /// </summary>
    /// 就是棵树
    public class SafeCompositePattern
    {
        public static void SafeCompositePatternMain()
        {
            var root = new ConcreteCompositeSafe("root");
            root.Add(new ConcreteLeafSafe("Leaf RA"));
            root.Add(new ConcreteLeafSafe("Leaf RB"));

            var compositeA = new ConcreteCompositeSafe("compasiteA");
            compositeA.Add(new ConcreteLeafSafe("Leaf CA"));
            compositeA.Add(new ConcreteLeafSafe("Leaf CB"));
            root.Add(compositeA);

            var compositeB = new ConcreteCompositeSafe("compasiteB");
            compositeB.Add(new ConcreteLeafSafe("Leaf CA"));
            compositeB.Add(new ConcreteLeafSafe("Leaf CB"));
            root.Add(compositeB);

            //list.Remove(Object)是通过比较引用地址
            //因此新new出的无法将List中的数据移除

            var leafC = new ConcreteCompositeSafe("Leaf RC");
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


        #region 安全式
        //可以有效的区分枝叶节点，且责任很明确 

        /// <summary>
        /// 抽象构件角色
        /// </summary>
        public abstract class ComponentSafe
        {
            protected string Name { get; set; }

            protected ComponentSafe(string name)
            {
                this.Name = name;
            }


            public abstract void Operation(int depth);
        }

        /// <summary>
        /// 抽象枝节点
        /// </summary>
        public abstract class CompositeSafe : ComponentSafe
        {
            protected List<ComponentSafe> components = new List<ComponentSafe>();

            protected CompositeSafe(string name) : base(name) { }

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
        public class ConcreteCompositeSafe : CompositeSafe
        {
            public ConcreteCompositeSafe(string name) : base(name) { }

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



}