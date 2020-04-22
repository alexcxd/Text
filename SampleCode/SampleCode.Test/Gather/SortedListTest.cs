using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SampleCode.Test.Gather
{
    /// <summary>
    /// 有序列表
    /// 定义：表示键/值对的集合，这些键和值按键排序并可按照键和索引访问。
    /// 特性：1.键不可重复，若键对应多个值可用Lookup；2.根据键进行排序；3.速度比HashTable慢。
    /// </summary>
    public class SortedListTest
    {
        /// <summary>
        /// 有序列表 基本操作
        /// </summary>
        [Test]
        public void SortedListCodeTest()
        {
            //初始化有序列表并设置容器容量
            var sortedList = new SortedList<string, string>(10);
            sortedList.Capacity = 4;

            //向有序表插入键值对,若插入已有键，报ArgumentException
            sortedList.Add("BBB", "BBB1");
            sortedList.Add("AAA", "AAA1");
            sortedList.Add("ZZZ", "ZZZ1");
            sortedList.Add("DDD", "DDD1");

            //可以通过索引器插入键值对,若插入已有键,则用新value代替旧value
            sortedList["CCC"] = "CCC1";

            //使用迭代器迭代有序列表
            foreach (KeyValuePair<string, string> keyValuePair in sortedList)
            {
                Console.WriteLine($"{keyValuePair.Key}:{keyValuePair.Value}");
            }
            foreach (var sortedListKey in sortedList.Keys)
            {
                Console.WriteLine($"Key:{sortedListKey}");
            }
            foreach (var sortedListValue in sortedList.Values)
            {
                Console.WriteLine($"Value:{sortedListValue}");
            }

            //查找某一键在有序列表中是否存在,
            string our1;
            var result1 = sortedList.TryGetValue("AAA", out our1);
            string our2;
            var result2 = sortedList.TryGetValue("TTT", out our2);
        }
    }
}