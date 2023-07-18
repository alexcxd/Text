using System;
using System.Collections.Generic;

namespace SampleCode.DesignPattern.BehavioralPatterns
{
    /// <summary>
    /// 迭代器模式
    /// </summary>
    /// 分离集合对象的遍历行为
    /// 应用：聚集对象需要迭代时,举例：IEnumerable接口
    public class IteratorPattern
    {
        public static void IteratorPatternMain()
        {
            ConcreteAggregate a = new ConcreteAggregate();

            a[0] = "1";
            a[1] = "2";
            a[2] = "3";
            a[3] = "4";
            a[4] = "5";
            a[5] = "6";
            a[6] = "7";
            a[7] = "8";
            a[8] = "9";

            var iterator = a.CreateIterator();

            while (!iterator.IsDone())
            {
                Console.WriteLine("{0}",iterator.Next());
            }
        }
    }

    public abstract class Iterator
    {
        /// <summary>
        /// /得到开始对象
        /// </summary>
        /// <returns></returns>
        public abstract object First();

        /// <summary>
        /// 得到下一个对象
        /// </summary>
        /// <returns></returns>
        public abstract object Next();

        /// <summary>
        /// 判断是否到结尾
        /// </summary>
        /// <returns></returns>
        public abstract bool IsDone();


    }

    public abstract class Aggregate
    {
        /// <summary>
        /// 创造迭代器
        /// </summary>
        /// <returns></returns>
        public abstract Iterator CreateIterator();
    }

    public class ConcreteIterator : Iterator
    {
        private readonly ConcreteAggregate aggregate;
        private int count = 0;

        public ConcreteIterator(ConcreteAggregate aggregate)
        {
            this.aggregate = aggregate;
        }



        public override object First()
        {
           
            return aggregate[0];
        }

        public override bool IsDone()
        {
            return count >= aggregate.Count;
        }

        public override object Next()
        {
            if (!(count < aggregate.Count))
            {
                throw new Exception("索引溢出");
            }
            var result = aggregate[count];
            count++;
            return result;
        }
    }

    public class ConcreteAggregate : Aggregate
    {
        private readonly IList<object> items = new List<object>();

        public int Count => items.Count;

        public object this[int index]
        {
            get => items[index];
            set => items.Add(value);
        }

        public override Iterator CreateIterator()
        {
            return new ConcreteIterator(this);
        }

    }

}