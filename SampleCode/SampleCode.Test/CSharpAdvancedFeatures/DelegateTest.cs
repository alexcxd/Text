using System;
using NUnit.Framework;

namespace SampleCode.Test.CSharpAdvancedFeatures
{
    public delegate int Transformer(int x);
    public delegate T Transformer<T>(T x);
    public delegate void ProgressReporter(int x);

    /// <summary>
    /// 委托
    /// </summary>
    [TestFixture]
    public class DelegateTest
    {
        /// <summary>
        /// 委托基本操作
        /// </summary>
        [Test]
        public void DelegateCodeTest()
        {
            //委托是一种知道如何调用方法的对象
            //委托类型定义了一种委托实例可以调用的方法, 即定义了返回类型和参数类型, 
            //例如 Transformer, 可以兼容任何然后类型为int并有一个int类型的参数的方法
            static int Square(int x) => x * x;
            Transformer t = Square;     //等价于 Transformer t = new Transformer(Square); 
            //调用方式
            var answer = t(3);          //d等价于 t.Invoke(3);
            Console.WriteLine(answer);  //9

            //使用委托书写插件方法
            //Util.Transform是一个高阶函数, 因为它是一个以函数为参数的方法
            var values = new int[]{ 1, 2, 3 };
            Util.Transform(values, Square);
            foreach(var value in values) 
            {
                Console.Write(value + " ");
            }

            //多播委托
            //所有的委托都具有多播功能, 即一个委托实例是可以引用一组方法的
            //注意事项：1.委托是按照添加顺序依次触发的
            //          2.其中委托是不可变的, 当调用+=或-=时实质上是创建一个新的委托实例, 并把它赋值给已有变量
            //          3.若一个多播委托有多个非void的返回值, 那么调用这会获得最后一个返回值
            static int Square2(int x) => x + x;
            Transformer t1 = Square;
            t1 += Square2;              //等价于 t1 = t1 + Square2;
            t1 -= Square2;              //等价于 t1 = t1 - Square2;

            //可以通过Target和Method获取最后一个方法的实例和函数(若为静态方法则Target为null)
            var x = new X();
            var y = new Y();
            ProgressReporter p = x.InstanceProgress1;
            p += y.InstanceProgress2;
            p(20);
            Console.WriteLine(p.Target == x);   //False
            Console.WriteLine(p.Target == y);   //True
            Console.WriteLine(p.Method);        //Void InstanceProgress(Int32)
        }

        /// <summary>
        /// 委托-泛型
        /// </summary>
        [Test]
        public void DelegateGenericityTest()
        {
            //委托类型可以包含泛型类型参数
            static int Square(int x) => x * x;
            var values = new int[] { 1, 2, 3 };
            Util.TransformGenericity(values, Square);
            foreach (var value in values)
            {
                Console.Write(value + " ");
            }

            //Func和Action委托是System下定义的两个通用小型委托
            //Func是具有任意参数和一个返回类型的委托
            //Action是具有任意参数没有返回类型的委托
        }

        delegate void D1();
        delegate void D2();
        delegate void StringAction(string s);
        delegate object ObjectRetriever();

        /// <summary>
        /// 委托-兼容性
        /// </summary>
        [Test]
        public void DelegateCompatibilityTest()
        {
            //类型的兼容性
            //即使签名相同, 委托类型也互不兼容
            static void Method1() { };
            D1 d1 = Method1;
            //D2 d2 = d1;       //Compile-time error
            D2 d2 = new D2(d1);
            //如果委托实例指向相同的目标方法, 则认为他们是等价的
            D1 d11 = Method1;
            D1 d12 = Method1;
            Console.WriteLine(d11 == d12);  //True]

            //参数的兼容性
            //委托可以有比它的目标类型方法参数类型更具体的参数类型, 这称为逆变
            static void ActOnObject(object o) => Console.WriteLine(o);
            StringAction sa = ActOnObject;
            sa("hello");    //hello

            //返回类型的兼容性
            //委托的目标方法可以返回比委托声明的返回值类型更加特定的返回值类型, 这称为协变
            static string RetrieveString() => "Hello";
            ObjectRetriever or = RetrieveString;
            Console.WriteLine(or());    //Hello

            //泛型委托类型的参数协变
            //若要定义一个泛型委托类型, 
            //将只用于返回值类型的类型参数标记为协变(out),
            //将只用于参数的任意类型参数标记为逆变(in),
        }
    }

    public class Util
    {
        public static void Transform(int[] values, Transformer t)
        {
            for(var i = 0; i < values.Length; i++)
            {
                values[i] = t(values[i]);
            }
        }

        public static void TransformGenericity<T>(T[] values, Transformer<T> t)
        {
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = t(values[i]);
            }
        }
    }

    public class X
    {
        public void InstanceProgress1(int percentComplete) => Console.WriteLine(percentComplete);
    }
    public class Y
    {
        public void InstanceProgress2(int percentComplete) => Console.WriteLine(percentComplete * percentComplete);
    }
}