using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SampleCode.DesignPattern.StructuralPatterns
{
    /// <summary>
    /// 组合模式 - 透明式
    /// </summary>
    /// 就是棵树
    public class TransparentCompositePattern
    {
        public static void TransparentCompositePatternMain()
        {

            var root = new Composite("root");
            root.Add(new Leaf("Leaf RA"));
            root.Add(new Leaf("Leaf RB"));

            var compositeA = new Composite("compasiteA");
            compositeA.Add(new Leaf("Leaf CA"));
            compositeA.Add(new Leaf("Leaf CB"));
            root.Add(compositeA);

            var compositeB = new Composite("compasiteB");
            compositeB.Add(new Leaf("Leaf CA"));
            compositeB.Add(new Leaf("Leaf CB"));
            root.Add(compositeB);

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
        public class Composite : Component
        {
            private List<Component> components = new List<Component>();

            public Composite(string name) : base(name) { }

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
    }
}