using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.Gather
{
    /// <summary>
    /// 自定义集合类和代理
    /// 
    /// </summary>
    [TestFixture]
    public class CollectionsTest
    {
        #region Collection基本操作

        /// <summary>
        /// Collection基本操作
        /// </summary>
        [Test]
        public void CollectionTest()
        {
            //Collection<T>类是一个可定制的List<T>包装类, 实现了IList<T>和IList
            //Collection<T>提供了类似钩子的四个虚方法(ClearItem、InsertItem、RemoveItem、SetItem)
            var zoo = new Zoo1();
            zoo.Animals.Add(new Animal1("Bird", 1));
            zoo.Animals[0] = new Animal1("Dog", 2);
            zoo.Animals.RemoveAt(0);
            zoo.Animals.Clear();
        }

        public class Animal1
        {
            public string name;
            public int popularity;
            public Zoo1 Zoo { get; internal set; }

            public Animal1(string name, int popularity)
            {
                this.name = name;
                this.popularity = popularity;
            }
        }

        public class AnimalCollection1 : Collection<Animal1>
        {
            private Zoo1 zoo;

            public AnimalCollection1(Zoo1 zoo)
            {
                this.zoo = zoo;
            }

            protected override void InsertItem(int index, Animal1 item)
            {
                base.InsertItem(index, item);
                item.Zoo = zoo;
            }

            protected override void SetItem(int index, Animal1 item)
            {
                base.SetItem(index, item);
                item.Zoo = zoo;
            }

            protected override void RemoveItem(int index)
            {
                this[index].Zoo = null;
                base.RemoveItem(index);
            }

            protected override void ClearItems()
            {
                foreach (var animal in this)
                {
                    animal.Zoo = null;
                }
                base.ClearItems();
            }
        }

        public class Zoo1
        {
            public readonly AnimalCollection1 Animals;

            public Zoo1()
            {
                Animals = new AnimalCollection1(this);
            }
        }

        #endregion

        #region KeyedCollection基本操作

        /// <summary>
        /// KeyedCollection基本操作
        /// </summary>
        [Test]
        public void KeyedCollectionTest()
        {
            //KeyedCollection<TKey, TItem>继承自Collection<TItem>, 它内部同时使用了线性表和散列表
            //它相对于Collection<TItem>增加了通过键访问元素的功能
            var zoo = new Zoo2();
            zoo.Animals.Add(new Animal2("Kangaroo", 10));
            zoo.Animals.Add(new Animal2("Mr Sea Lion", 20));
            Console.WriteLine(zoo.Animals[0].popularity);
            Console.WriteLine(zoo.Animals["Mr Sea Lion"].popularity);   //20
            zoo.Animals["Kangaroo"].Name = "Mr Roo";
            Console.WriteLine(zoo.Animals["Mr Roo"].popularity);   //10
        }

        public class Animal2
        {
            private string name;

            public string Name
            {
                get => name;
                set
                {
                    //当一个元素的键发生变化时，必须使用ChangeItemKey方法更新内部的字典
                    Zoo.Animals.NotifyNameChange(this, value);
                    name = value;
                }
            }

            public int popularity;
            public Zoo2 Zoo { get; internal set; }

            public Animal2(string name, int popularity)
            {
                this.name = name;
                this.popularity = popularity;
            }
        }

        public class AnimalCollection2 : KeyedCollection<string, Animal2>
        {
            private Zoo2 zoo;

            public AnimalCollection2(Zoo2 zoo)
            {
                this.zoo = zoo;
            }

            internal void NotifyNameChange(Animal2 animal, string newName)
            {
                this.ChangeItemKey(animal, newName);
            }

            //获取底部对象键的方法
            protected override string GetKeyForItem(Animal2 item)
            {
                return item.Name;
            }

            protected override void InsertItem(int index, Animal2 item)
            {
                base.InsertItem(index, item);
                item.Zoo = zoo;
            }

            protected override void SetItem(int index, Animal2 item)
            {
                base.SetItem(index, item);
                item.Zoo = zoo;
            }

            protected override void RemoveItem(int index)
            {
                this[index].Zoo = null;
                base.RemoveItem(index);
            }

            protected override void ClearItems()
            {
                foreach (var animal in this)
                {
                    animal.Zoo = null;
                }
                base.ClearItems();
            }
        }

        public class Zoo2
        {
            public readonly AnimalCollection2 Animals;

            public Zoo2()
            {
                Animals = new AnimalCollection2(this);
            }
        }

        #endregion

        #region ReadOnlyCollection基本操作

        /// <summary>
        /// ReadOnlyCollection基本操作
        /// </summary>
        [Test]
        public void ReadOnlyCollectionTest()
        {
            //ReadOnlyCollection<T>是一个包装器(代理),它为集合提供了一种只读视图。
            var test = new Test();
            test.AddInternally("1");
            test.AddInternally("2");
            test.AddInternally("3");

        }

        public class Test
        {
            private List<string> names;
            public ReadOnlyCollection<string> Names { get; }

            public Test()
            {
                names = new List<string>();
                Names = new ReadOnlyCollection<string>(names);
            }

            public void AddInternally(string name)
            {
                names.Add(name);
            }
        }

        #endregion
    }
}