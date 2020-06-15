using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SampleCode.Test.Gather
{
    /// <summary>
    /// 字典，又称散列表、映射表
    /// 键是通过GetHashCode生成，以将键散列在一个表中
    /// </summary>
    public class DictionaryTest
    {
        /// <summary>
        /// Dictionary基本操作
        /// </summary>
        [Test]
        public void DictionaryCodeTest()
        {
            //Dictionary使用散列表存储数据

            var dictionary = new Dictionary<DictionaryKey, string>();

            //插入
            dictionary.Add(new DictionaryKey("AAA"), "AAA1");
            dictionary.Add(new DictionaryKey("BBB"), "BBB1");
            dictionary[new DictionaryKey("CCC")] = "CCC1";

            //移除
            var aaa = dictionary.Remove(new DictionaryKey("AAA"));
            var zzz = dictionary.Remove(new DictionaryKey("ZZZ"));

            //确认字典中是否存在某一键值
            var result1 = dictionary.TryGetValue(new DictionaryKey("CCC"), out string out1);
            var result2 = dictionary.TryGetValue(new DictionaryKey("VVV"), out string out2);

            //遍历
            foreach (KeyValuePair<DictionaryKey, string> keyValuePair in dictionary)
            {
                Console.WriteLine($"{keyValuePair.Key}:{keyValuePair.Value}");
            }
            foreach (var key in dictionary.Keys)
            {
                Console.WriteLine($"Key:{key}");
            }
            foreach (var value in dictionary.Values)
            {
                Console.WriteLine($"Key:{value}");
            }
        }

        /// <summary>
        /// Lookup基本操作
        /// </summary>
        [Test]
        public void LookupTest()
        {
            //Lookup和Dictionary很相似，但是他是将键映射到一个值集上。
            //Lookup无法直接初始化，只能通过ToLookup创建。

            var person = new List<Person>
            {
                new Person("A","AAA"),
                new Person("A","BBB"),
                new Person("A","CCC"),
                new Person("B","DDD"),
            };

            var lookup = person.ToLookup(m => m.Country);
        }

        /// <summary>
        /// 有序字典 SortedDictionary基本操作
        /// </summary>
        [Test]
        public void SortedDictionaryTest()
        {
            //有序字典是通过红黑树实现的
            //有序字典的键必须实现IComparable<TKey>接口，
            //如果未实现可以创建一个实现IComparable<TKey>接口的比较器作为SortedDictionary构造函数的参数
            //SortedDictionary在随机序列中插入元素的速度比SortedList快的多
            var sortedDictionary = new SortedDictionary<SortedDictionaryKey, string>();
            sortedDictionary.Add(new SortedDictionaryKey("BBB"), "BBB1");
            sortedDictionary.Add(new SortedDictionaryKey("AAA"), "AAA1");
            sortedDictionary.Add(new SortedDictionaryKey("ZZZ"), "ZZZ1");
            sortedDictionary.Add(new SortedDictionaryKey("DDD"), "DDD1");
            sortedDictionary.Remove(new SortedDictionaryKey("AAA"));
        }
    }

    //作为Dictionary的键必须实现Equals和GetHashCode
    public class DictionaryKey
    {
        public DictionaryKey(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return Key.Equals(((DictionaryKey)obj).Key);
        }

        public override int GetHashCode()
        {
            var a = Key.GetHashCode();
            return Key.GetHashCode();
        }

        public override string ToString()
        {
            return Key;
        }
    }

    public class Person
    {
        public string Country { get; set; }
        public string Name { get; set; }

        public Person(string country, string name)
        {
            Country = country;
            Name = name;
        }
    }

    public class SortedDictionaryKey : IComparable<SortedDictionaryKey>
    {

        public SortedDictionaryKey(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return Key.Equals(((SortedDictionaryKey)obj).Key);
        }

        public override int GetHashCode()
        {
            var a = Key.GetHashCode();
            return Key.GetHashCode();
        }

        public override string ToString()
        {
            return Key;
        }

        public int CompareTo(SortedDictionaryKey other)
        {
            var ascEncoding = new ASCIIEncoding();
            if (ascEncoding.GetBytes(Key).First() == ascEncoding.GetBytes(other.Key).First())
            {
                return 0;
            }
            if (ascEncoding.GetBytes(Key).First() > ascEncoding.GetBytes(other.Key).First())
            {
                return 1;
            }
            if (ascEncoding.GetBytes(Key).First() < ascEncoding.GetBytes(other.Key).First())
            {
                return -1;
            }
            return string.Compare(Key, other.Key, StringComparison.Ordinal);
        }
    }
}