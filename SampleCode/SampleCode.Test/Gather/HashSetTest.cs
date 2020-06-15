using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SampleCode.Test.Gather
{
    public class HashSetTest
    {
        /// <summary>
        /// HashSet：不重复的集合
        /// </summary>
        [Test]
        public void HashSetCodeTest()
        {
            //HashSet的特点: 1. 它的Contains方法均使用散列查找因此执行速度很快
            //               2. 它不会保存重复的元素, 并且会忽视重复值的请求
            //               3. 无法根据为位置访问元素
            //HastSet是通过使用存储键的散列表实现

            var alphabets1 = new HashSet<string> { "A", "V", "R", "Q" };
            var alphabets2 = new HashSet<string> { "A", "T", "R", "B" };
            var alphabets3 = new HashSet<string> { "A", "V" };

            //插入
            //Add返回一个bool类型，若插入成功返回true，反之返回false
            var result1 = alphabets1.Add("A");
            var result2 = alphabets2.Add("C");

            //移除
            var result3 = alphabets2.Remove("C");

            var isProperSubsetOf = alphabets3.IsProperSubsetOf(alphabets1);     //真子集:alphabets3是否为alphabets1的子集
            var isSubsetOf = alphabets3.IsSubsetOf(alphabets1);                 //子集
            var isProperSupersetOf = alphabets1.IsProperSupersetOf(alphabets3); //真超集
            var isSupersetOf = alphabets1.IsSupersetOf(alphabets3);             //超集
            var overlaps = alphabets1.Overlaps(alphabets2);                     //两集合中是否存在相同的元素

            //并集
            alphabets1 = new HashSet<string> { "A", "V", "R", "Q" };
            alphabets2 = new HashSet<string> { "A", "T", "R", "B" };
            alphabets1.UnionWith(alphabets2);

            //差集
            alphabets1 = new HashSet<string> { "A", "V", "R", "Q" };
            alphabets2 = new HashSet<string> { "A", "T", "R", "B" };
            alphabets1.ExceptWith(alphabets2);

            //对称差集
            alphabets1 = new HashSet<string> { "A", "V", "R", "Q" };
            alphabets2 = new HashSet<string> { "A", "T", "R", "B" };
            alphabets1.SymmetricExceptWith(alphabets2);

            //交集
            alphabets1 = new HashSet<string> { "A", "V", "R", "Q" };
            alphabets2 = new HashSet<string> { "A", "T", "R", "B" };
            alphabets1.IntersectWith(alphabets2);
        }


        /// <summary>
        /// SortedSet：有序的不重复的集合
        /// </summary>
        [Test]
        public void SortedSetCodeTest()
        {
            //SortedSet是有序的HashSet, 
            //SortedSet是通过一个红黑树实现的
            var letters = new SortedSet<char>("the quick brown fox");
            foreach (var letter in letters)
            {
                Console.Write(letter);
            }
            Console.WriteLine();

            //可以通过GetViewBetween获取指定范围的结果
            foreach (var letter in letters.GetViewBetween('f', 'j'))
            {
                Console.Write(letter);
            }
            Console.WriteLine();
        }
    }
}