using System;
using static DotNetTest.访问级别.ProtectedTest;

namespace DotNetTest.访问级别
{
    public class AccessLevelMain
    {
        public void WriteLine()
        {
            ChildTest childTest = new ChildTest();

            //因为再BaseTest中PrintTest是未定义的，但他是ChildTest的父类，所以可以强转但是无法使用PrintTest，否则会报null异常
            //ChildTest childTest1 = new BaseTest() as ChildTest;  
            childTest.PrintTest();
        }
    }
}