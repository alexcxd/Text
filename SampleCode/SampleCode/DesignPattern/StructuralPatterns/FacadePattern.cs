using System;

namespace SampleCode.DesignPattern.StructuralPatterns
{
    /// <summary>
    /// 外观模式
    /// </summary>
    /// 通过提供一个统一接口，来访问子系统的多个接口。
    public class FacadePattern
    {
        public static void FacadePatternMain()
        {
            var facade = new Facade();
            facade.MethodAOne();
            facade.MethodBTwo();
        }

        #region 外观模式

        /// <summary>
        /// 外观类
        /// </summary>
        public class Facade
        {
            private SubSystemA aSystem;
            private SubSystemB bSystem;
            private SubSystemC cSystem;
            private SubSystemD dSystem;

            public Facade()
            {
                aSystem = new SubSystemA();
                bSystem = new SubSystemB();
                cSystem = new SubSystemC();
                dSystem = new SubSystemD();
            }

            /// <summary>
            /// 子系统A操作1
            /// </summary>
            public void MethodAOne()
            {
                Console.WriteLine("-----子系统A操作1-----");
                aSystem.MethodOne();
            }

            /// <summary>
            /// 子系统B操作2
            /// </summary>
            public void MethodBTwo()
            {
                Console.WriteLine("-----子系统B操作2-----");
                bSystem.MethodTwo();
            }
        }

        /// <summary>
        /// 子系统A
        /// </summary>
        class SubSystemA
        {
            /// <summary>
            /// 操作1
            /// </summary>
            public void MethodOne()
            {
                Console.WriteLine("子系统方法一");
            }
        }

        /// <summary>
        /// 子系统B
        /// </summary>
        public class SubSystemB
        {
            /// <summary>
            /// 操作2
            /// </summary>
            public void MethodTwo()
            {
                Console.WriteLine("子系统方法二");
            }
        }

        /// <summary>
        /// 子系统C
        /// </summary>
        public class SubSystemC
        {
            /// <summary>
            /// 操作3
            /// </summary>
            public void MethodThree()
            {
                Console.WriteLine("子系统方法三");
            }
        }

        /// <summary>
        /// 子系统D
        /// </summary>
        public class SubSystemD
        {
            /// <summary>
            /// 操作4
            /// </summary>
            public void MethodFour()
            {
                Console.WriteLine("子系统方法四");
            }
        }

        #endregion
    }

}
