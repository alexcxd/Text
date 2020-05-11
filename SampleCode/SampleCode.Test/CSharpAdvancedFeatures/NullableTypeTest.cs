using System;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.CSharpAdvancedFeatures
{
    /// <summary>
    /// 可空类型
    /// </summary>
    [TestFixture()]
    public class NullableTypeTest
    {
        /// <summary>
        /// 可空类型基本操作
        /// </summary>
        [Test]
        public void NullableTypeCodeTest()
        {
            //可空类型是由数据类型后加一个?来表示的
            //可空类型主要用于值类型
            int? i = null;
            Console.WriteLine(i == null);   //True

            //T?会在编译器中被翻译为结构体System.Nullable<T>, 它只有两个字段Value和HasValue
            //当HashValue为false时(即可空类型为空时),调用Value会抛出InvalidOperationException异常
            Nullable<int> i1 = new Nullable<int>();
            i1 = 1;
            Console.WriteLine(i1.Value);

            //T?的默认值为null
            Console.WriteLine(default(int?) == null);   //null

            //从T到T?是可以隐式转化的, 反之则为显式转化
            //显示转化和调用Value是等价的
            var i2 = 0;
            int? i3 = i2;
            var i4 = (int)i3;


            //可空类型和null运算符
            //在可空类型上可以通过使用??提供一个显示的默认值, 相当于调用GetValueOrDefault方法
            int? x1 = null;
            var y1 = x1 ?? 0;
        }

        /// <summary>
        /// 可空类型和运算符
        /// </summary>
        [Test]
        public void NullableTypeOperatorTest()
        {
            //可空类型和运算符
            int? x = 5;
            int? y = null;
            int? z = 10;

            //1.相等运算符(==和!=)
            //如果只有一个操所数为null时两个操作数不相等,如果两个操作数都不为null则比较两者的Value
            Console.WriteLine(x == y);  //False 
            Console.WriteLine(x == z);  //False

            //2.关系运算符
            //对于关系运算符比较null是没有意义的, 因此表空值和另外一个空值或者非空值都是false
            //b和b1等价
            var b = x < y;  //False
            var b1 = (x.HasValue && y.HasValue) ? (x.Value < y.Value) : false;

            //3.其他运算符(+、-、*、/、%、&、|、^、<<、>>、+、++、--、! 和~)
            //当任意一个操作数为null时, 则这些运算符都会返回null
            var c = x + y;  //null
            var c1 = (x.HasValue && y.HasValue) ? (int?)(x.Value + y.Value) : null;

            //4.上述的例外, 在bool?上使用&和|
            //在bool?中使用&或|, 操作数的null会被认为是一个未知值, 未知值不会影响必然的结果
            bool? n = null;
            bool? f = false;
            bool? t = true;
            Console.WriteLine(n | n);   //null
            Console.WriteLine(n | f);   //null
            Console.WriteLine(n | t);   //True
            Console.WriteLine(n & n);   //null
            Console.WriteLine(n & f);   //False
            Console.WriteLine(n & t);   //null
        }
    }
}