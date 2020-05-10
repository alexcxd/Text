using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace SampleCode.Test.CSharpAdvancedFeatures
{
    /// <summary>
    /// 可枚举类型和迭代器
    /// </summary>
    [TestFixture]
    public class EnumeratorTest
    {
        /// <summary>
        /// 可枚举类型
        /// </summary>
        [Test]
        public void EnumeratorCodeTest()
        {
            //枚举器是一个只读的且只能在值序列上前移的游标(即具有MoveNext方法和Current属性的对象)
            //枚举器需要实IEnumerator或IEnumerator<T>两个接口之一
            //foreach语句用来在可枚举的对象上执行迭代操作
            foreach (var c in "beer")
            {
                Console.Write(c + " ");
            }   //b e e r
            //以下写法等价于foreach, foreach是它的简化
            using (var enumerator = "beer".GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var element = enumerator.Current;
                    Console.Write(element + "");
                }
            }

            //集合初始化器
            //集合初始化器需要可枚举对象实现IEnumerable接口,并且有可以调用的带适当参数的Add方法
            var list = new List<int> { 1, 2, 3 };
            var a = list[1];
            //编译器会将上述代码翻译为
            var list1 = new List<int>();
            list1.Add(1);
            list1.Add(2);
            list1.Add(3);
            //可以在具有索引器的类型指定元素位置
            var dict = new Dictionary<int, string>()
            {
                [0] = "one",
                [1] = "two",
                [2] = "three"
            };
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        [Test]
        public void YieldTest()
        {
            //迭代器是枚举器的生产者
            //迭代器是包含一个或者多个yield语句的方法、属性或者索引器
            //迭代器必须返回IEnumerable或者IEnumerator
            //迭代器编译器实现：
            //在编译器中迭代方法会被转化为实现了IEnumerable<T>或IEnumerator<T>的私有类
            //迭代器块中的逻辑被“反转”并分别进入编译器生成的枚举器类的MoveNext方法和Current属性。
            //当调用迭代器方法的时候，所做的仅仅是实例化编译器生成的类，而迭代器代码并没有真正执行。
            //编写的迭代器代码只有当开始枚举结果序列时才开始执行(即执行MoveNext时)

            //yield return语句表示这是当前枚举器产生的下一个元素
            var fids = GetFibs(6);
            foreach (var fid in fids)
            {
                Console.Write(fid + " ");
            }

            //yield break语句表明迭代器块不再返回更多的元素而是提前退出(迭代器中不允许出现return)
            var foo = Foo(false);

            //只有当try语句块只带finally时才可以在try语句块中使用yield
            //无法在有catch语句块代码中使用的原因是在编译器转化时异常处理语句会大大的增加代码的复杂度
            var foo1 = Foo1();
        }

        /// <summary>
        /// 返回指定数量的斐波那契数列
        /// </summary>
        /// <param name="fidCount"></param>
        /// <returns></returns>
        public IEnumerable<int> GetFibs(int fidCount)
        {
            for (int i = 0, prevFid = 1, curFid = 1; i < fidCount; i++)
            {
                yield return curFid;
                var newFid = prevFid + curFid;
                prevFid = curFid;
                curFid = newFid;
            }
        }

        public IEnumerable<string> Foo(bool breakEarly)
        {
            yield return "one";
            yield return "two";

            if (breakEarly)
            {
                yield break;
            }

            yield return "three";
        }
        public IEnumerable<string> Foo1()
        {
            try
            {
                yield return "one";
                yield return "two";
            }
            finally
            {

            }
        }
    }
}