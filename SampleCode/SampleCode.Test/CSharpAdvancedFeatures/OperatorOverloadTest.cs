using System;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.CSharpAdvancedFeatures
{
    /// <summary>
    /// 运算符重载
    /// </summary>
    [TestFixture]
    public class OperatorOverloadTest
    {
        /// <summary>
        /// 运算符重载 基本操作
        /// </summary>
        [Test]
        public void OperatorOverloadCodeTest()
        {
            //运算符可以通过重载以更自然的方式操作自定义类型, 很适合结构体
            //可重载的运算符：+(一元), -(一元), !, ?, ++, --, +, -, *, /,
            //                %, &, |, ^, <<, >>, ==, !=, >, <, >=, <=,
            //                隐式转换, 显示转换, true, false
            //其中+=和-=可以通过+和-实现, &&和||可以通过&和|实现

            //运算符重载基本规则：
            //1.函数名必须为operator关键字跟上运算符符号
            //2.运算符函数必须是static和public的
            //3.运算符的参数即使操所数
            //4.运算符函数的返回结果代表着表达式的结果
            //5.运算符函数的参数至少有一个类型和声明运算符函数的类型一致
            var note = new Note(1);
            note = note + 2;
            note += 3;

        }

        public struct Note
        {
            private int value;
            public Note(int value) => this.value = value;

            public static Note operator +(Note x, int value) => new Note(x.value + value);
        }

        /// <summary>
        /// 运算符重载 重载比较运算符
        /// </summary>
        [Test]
        public void ComparisonOperatorOverloadTest()
        {
            //重载比较运算符的规则：
            //1.需要成对重载(==和！=)、(>和<)、(>=和<=)
            //2.在重载==或者!=运算符时, 一般也会重载Equal和GetHashCode
            //3.在重载比较运算符时,  一般也会重载IComparable, IComparable<T>
            //重载Equal和GetHashCode以及IComparable是为了保证比较上的一致性

            var person = new Person();
            person.Age = 22;
            var b1 = person == 22;  //true
            var b2 = person != 22;  //false
            var b3 = person > 21;   //true
            var b4 = person < 23;   //true
            var b5 = person.CompareTo(20); //-1
        }

        public struct Person : IComparable
        {
            public int Age { get; set; }

            #region 重载==和!=

            public static bool operator ==(Person p, int age) => p.Age == age;
   
            public static bool operator !=(Person p, int age) => p.Age != age;

            public bool Equals(Person other)
            {
                return Age == other.Age;
            }

            public override bool Equals(object obj)
            {
                return obj is Person other && Equals(other);
            }

            public override int GetHashCode()
            {
                return Age;
            }

            #endregion

            #region 重载>和<

            public static bool operator >(Person p, int age)
            {
                return p.Age > age;
            }

            public static bool operator <(Person p, int age)
            {
                return p.Age < age;
            }

            public int CompareTo(object obj)
            {
                var age = (int)obj;

                if (age > Age)
                {
                    return 1;
                }
                else if (age < Age)
                {
                    return -1;
                }

                return 0;
            }

            #endregion
        }

        /// <summary>
        /// 运算符重载 自定义隐式和显示转换
        /// </summary>
        [Test]
        public void ImplicitAndDisplayConversionTest()
        {
            //在定义隐式和显示转换一般用于强相关的类型(例如数字)
            //如果是弱相关的类型可以采用编写一个以转换类型为参数的构造函数或者编写静态ToXXX的函数
            var n = new Note1(1.1);
            double d = n;
            Note1 n1 = (Note1)d;

            //as和is会忽视自定义转换
            var b = 1.1 is Note1;   //false
            //var n = 1.1 as Note1;   //Error
        }

        public struct Note1
        {
            private double value;
            public Note1(double value) => this.value = value;

            public static implicit operator double(Note1 note1)
            {
                return note1.value;
            }
            public static explicit operator Note1(double d)
            {
                return new Note1(d);
            }
        }


        /// <summary>
        /// 运算符重载 重载true和false
        /// </summary>
        [Test]
        public void TrueAndFalseOperatorOverloadTest()
        {
            //true和false运算符只会在那些本身具有布尔语义但又无法转换为bool的类型中重载
            var a = SqlBoolean.Null;
            if (a)
            {
                Console.WriteLine("True");
            }
            else if (!a)
            {
                Console.WriteLine("False");
            }
            else
            {
                Console.WriteLine("Null");
            }
        }

        public struct SqlBoolean
        {
            private byte m_value;
            private SqlBoolean(byte b) => m_value = b;

            public static readonly SqlBoolean Null = new SqlBoolean(0);
            public static readonly SqlBoolean False = new SqlBoolean(1);
            public static readonly SqlBoolean True = new SqlBoolean(2);

            public static bool operator true (SqlBoolean x) => x.m_value == True.m_value;
            public static bool operator false (SqlBoolean x) => x.m_value == False.m_value;
            public static SqlBoolean operator ! (SqlBoolean x)
            {
                if (x.m_value == Null.m_value) return Null;
                return x.m_value == True.m_value ? True : False;
            }
        }
    }
}