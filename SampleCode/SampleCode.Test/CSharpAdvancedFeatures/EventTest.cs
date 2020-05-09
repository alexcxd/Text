using System;
using NUnit.Framework;

namespace SampleCode.Test.CSharpAdvancedFeatures
{
    /// <summary>
    /// 事件
    /// </summary>
    [TestFixture]
    public class EventTest
    {
        /// <summary>
        /// 事件基本操作
        /// </summary>
        [Test]
        public void EventCodeTest()
        {
            //事件是一种使用有限委托功能实现订阅者/广播者模式的结构
            //声明事件使用关键字event 例如：Stock.PriceChange
            //如果不在委托上使用事件, PriceChange将变成普通的委托, 虽然运行结果不会改变, 但是订阅者之间将会互相影响
            //1.直接通过=指定委托(会将其他订阅者的委托覆盖掉)
            //2.清除所有委托(将委托设置未null)
            //3.通过调用其委托广播到其他订阅者
            var stock = new Stock("Product1");
            static void PriceChange(decimal oldPrice, decimal newPrice) => Console.WriteLine("Change");
            stock.PriceChange += PriceChange;
            stock.Price = 10;
            stock.PriceChange -= PriceChange;
            stock.Price = 20;
        }

        public delegate void PriceChangeHandler(decimal oldPrice, decimal newPrice);

        public class Stock
        {
            private string symbol;
            private decimal price;

            public Stock(string symbol) => this.symbol = symbol;

            public event PriceChangeHandler PriceChange;

            //以下写法等价于PriceChange, 在编译器中会将代码翻译如下的形式
            //PriceChanged被称为事件访问器，
            private PriceChangeHandler priceChanged;
            public event PriceChangeHandler PriceChanged
            {
                add => priceChanged += value;
                remove => priceChanged -= value;
            }

            public decimal Price
            {
                get => price;
                set
                {
                    if (price == value)
                    {
                        return;
                    }

                    var oldPrice = price;
                    price = value;
                    PriceChange?.Invoke(oldPrice, price);
                }
            }
        }

        /// <summary>
        /// 标准事件模型
        /// </summary>
        [Test]
        public void EventArgsTest()
        {
            //标准事件模型是.Net为事件编程定义的一个标准模型
            //标准事件模型的核心是System.EventArgs, 它是为事件传递信息的基类
            var stock = new Stock1("Product1");
            static void PriceChange(object sender, PriceChangedEventArgs e)
            {
                var stock = sender as Stock1;
                Console.WriteLine($"The price goes from {e.lastPrice} to {e.newPrice}");
            };
            stock.PriceChanged += PriceChange;
            stock.Price = 10;
            stock.PriceChanged -= PriceChange;
            stock.Price = 20;

            //显式定义事件访问器，可以在委托的存储和访问上进行更复杂的操作
            //在以下三种情形可能需要
            //1.当前事件访问器仅仅是广播事件类的中继器
            //2.当类定义了大量的事件, 而大部分事件都只有很少的订阅者,这种情况最好在字典中存储订阅者,
            //  因为字典比空委托字段的存储开销更小
            //3.当显式实现声明事件的接口时，例如 Foo
        }

        //用于传递信息
        public class PriceChangedEventArgs : EventArgs
        {
            public readonly decimal lastPrice;
            public readonly decimal newPrice;

            public PriceChangedEventArgs(decimal lastPrice, decimal newPrice)
            {
                this.lastPrice = lastPrice;
                this.newPrice = newPrice;
            }
        }

        public class Stock1
        {
            private string symbol;
            private decimal price;

            public Stock1(string symbol) => this.symbol = symbol;

            //EventHandler是定义在System下的泛型委托
            public event EventHandler<PriceChangedEventArgs> PriceChanged;

            //委托的触发方法,方法名必须与事件名一致并以On作为前缀
            protected virtual void OnPriceChanged(PriceChangedEventArgs e)
            {
                PriceChanged?.Invoke(this, e);
            }

            public decimal Price
            {
                get => price;
                set
                {
                    if (price == value)
                    {
                        return;
                    }

                    var lastPrice = price;
                    price = value;
                    OnPriceChanged(new PriceChangedEventArgs(lastPrice, price));
                }
            }
        }

        public interface IFoo
        {
            event EventHandler Ev;
        }

        public class Foo : IFoo
        {
            public event EventHandler ev;
            public event EventHandler Ev
            {
                add => Ev += value;
                remove => Ev -= value;
            }
        }
    }

}