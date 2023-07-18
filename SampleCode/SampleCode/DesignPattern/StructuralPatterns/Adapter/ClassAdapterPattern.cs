using System;

namespace SampleCode.DesignPattern.StructuralPatterns
{
    /// <summary>
    /// 适配器模式-类适配器
    /// </summary>
    /// 类适配器：这一实现使用了继承机制： 适配器同时继承两个对象的接口。 请注意， 这种方式仅能在支持多重继承的编程语言中实现。
    public class ClassAdapterPattern
    {
        public static void ObjectAdapterPatternMain()
        {
            Target adapter = new Adapter();
            adapter.Request();
        }

        /// <summary>
        /// 
        /// </summary>
        public interface ITarget
        {
            void Request();
        }

        /// <summary>
        /// 需要接口
        /// </summary>
        public class Target : ITarget
        {
            public void Request()
            {
                Console.WriteLine("普通请求");
            }
        }

        /// <summary>
        /// 现存接口
        /// </summary>
        public class Adaptee
        {
            public void SpecificRequest()
            {
                Console.WriteLine("特殊请求");
            }
        }

        /// <summary>
        /// 转换器
        /// </summary>
        public class Adapter : Adaptee, ITarget
        {
            public  void Request()
            {
                this.SpecificRequest();
            }
        }
    }
}