using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 练习.AttributeTest
{
    /*
     * 特性是用来向程序添加声明性信息
     * 要获取某个类的特性，需要通过反射实现
     */
    class AttributeDemo1
    {
        public static void AttributeDemo1Main()
        {
            //反射取出方法或者类的特性
            MethodInfo info = typeof(ReflectTest).GetMethod("AttributeDemo");

            var attribute = Attribute.GetCustomAttribute(info, typeof(TestAttribute)) as TestAttribute;

            //不适用于事件和属性
            var attribute1 = info.GetCustomAttributes(typeof(TestAttribute));

            var type = info.CustomAttributes.ToList();
            var attribute2 = Activator.CreateInstance(type[0].AttributeType);
        }
    }

    public class ReflectTest
    {
        //[特性名(构造函数中的参数, 可选参数)]
        //等价于 [TestAttribute("TestAttribute", A = 1, B = 2, C = 3)]
        [Test("TestAttribute", 3.1415, A = 1, B = 2, C = 3)]
        public void AttributeDemo()
        {

        }
    }

    //AttributeTargets中记录着自定义特性可以作用的类型，Method为方法，Field为字段，All为任何
    //AllowMultiple表示一个特性可否多次用在同一项上，Inherited表示当前特性是否可以被派生类或者接口应用
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class TestAttribute : Attribute
    {
        private string str;
        private double d;

        //可选参数
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }

        //构造函数
        public TestAttribute(string str, double d)
        {
            this.str = str;
            this.d = d;
        }

        public TestAttribute()
        {
            
        }
    }

}
