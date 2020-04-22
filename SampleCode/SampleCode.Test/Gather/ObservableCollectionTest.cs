using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using NUnit.Framework;

namespace SampleCode.Test.Gather
{
    /// <summary>
    /// 可观察的集合
    /// 可以查看集合中元素何时删除或添加信息的集合
    /// </summary>
    public class ObservableCollectionTest
    {
        [Test]
        public void ObservableCollectionCodeTest()
        {
            var data = new ObservableCollection<string>();
            //CollectionChanged是可观察集合的修改触发委托
            data.CollectionChanged += Data_CollectionChanged;
            data.Add("AAA");
            data.Add("BBB");
            data.Insert(1, "ZZZ");
            data.Add("CCC");
            data.Remove("CCC");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">触发行为的对象</param>
        /// <param name="e">发生的行为</param>
        private static void Data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine($"Action:{e.Action}");

            if (e.OldItems != null)
            {
                Console.WriteLine($"start index for old item(s): {e.OldStartingIndex}");
                foreach (var eOldItem in e.OldItems)
                {
                    Console.WriteLine(eOldItem);
                }
            }

            if (e.NewItems != null)
            {
                Console.WriteLine($"start index for new item(s): {e.NewStartingIndex}");
                foreach (var eNewItem in e.NewItems)
                {
                    Console.WriteLine(eNewItem);
                }
            }

            Console.WriteLine();
        }
    }
}