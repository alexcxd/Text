using System;

namespace SampleCode.DesignPattern.结构型模式
{
    /// <summary>
    /// 外观模式
    /// </summary>
    /// 通过提供一个统一接口，来访问子系统的多个接口。
    public class FacadePattern
    {
        public static void FacadePatternMain()
        {
            Facade facade = new Facade();
            facade.MethodA();
            facade.MethodB();
        }
    }

    class SubSystemOne
    {
        public void MethodOne()
        {
            Console.WriteLine("子系统方法一");
        }
    }

    class SubSystemTwo
    {
        public void MethodTwo()
        {
            Console.WriteLine("子系统方法二");
        }
    }

    class SubSystemThree
    {
        public void MethodThree()
        {
            Console.WriteLine("子系统方法三");
        }
    }

    class SubSystemFour
    {
        public void MethodFour()
        {
            Console.WriteLine("子系统方法四");
        }
    }

    /// <summary>
    /// 外观类
    /// </summary>
    public class Facade
    {
        private SubSystemOne one;
        private SubSystemTwo two;
        private SubSystemThree three;
        private SubSystemFour four;

        public Facade()
        {
            one = new SubSystemOne();
            two = new SubSystemTwo();
            three = new SubSystemThree();
            four = new SubSystemFour();
        }

        public void MethodA()
        {
            Console.WriteLine("-----方法组A-----");
            one.MethodOne();
            two.MethodTwo();
            three.MethodThree();
            four.MethodFour();
        }


        public void MethodB()
        {
            Console.WriteLine("-----方法组B-----");
            one.MethodOne();
            three.MethodThree();
            four.MethodFour();
        }
    }
}
