using System;

namespace DotNetTest.访问级别
{
    public class ProtectedTest
    {
        public class BaseTest
        {
            protected int A = 1;
            protected int B = 2;
        }

        public class ChildTest : BaseTest
        {
            public int C;
            public int D;

            public ChildTest(){}

            public void PrintTest()
            {
                BaseTest baseTest = new BaseTest();

                //如果直接从baseTest去获取A会报错，原因是不是从派生类获取而是从基类获取
                //this.C = baseTest.A

                this.C = base.A;
                Console.WriteLine(this.C);
            }
        }

    }
}