using System;

namespace SampleCode.DesignPattern.StructuralPatterns
{
    /// <summary>
    /// 适配器模式
    /// </summary>
    /// 将一个类转化为客户需要的另一个接口
    /// 主要应用于希望应用一些现存类，但是接口又与复用环境不符合的情况
    public class AdapterPattern
    {
        public static void AdapterPatternMian()
        {
            Target adapter = new Adapter();
            adapter.Request();
        }
    }

    //需要接口
    public class Target
    {
        public virtual void Request()
        {
            Console.WriteLine("普通请求");
        }
    }

    //现存接口
    public class Adaptee
    {
        public void SpecificRequest()
        {
            Console.WriteLine("特殊请求");
        }
    }

    //转换器
    public class Adapter : Target
    {
        Adaptee target = new Adaptee();

        public override void Request()
        {
            target.SpecificRequest();
        }
    }
}
