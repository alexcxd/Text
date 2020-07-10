using System;
using System.IO;
using System.IO.Compression;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.CSharpAdvancedFeatures
{
    /// <summary>
    /// 对象销毁相关操作
    /// </summary>
    [TestFixture]
    public class DisplayTest
    {
        #region IDisposable接口和Display方法基本操作

        /// <summary>
        /// IDisposable接口和Display方法基本操作
        /// </summary>
        [Test]
        public void DisplayCodeTest()
        {
            //IDisposable是Framework提供销毁操作的接口, 内部包含Display方法
            //using语句提供了调用Display方法的捷径
            using (var fs = new FileStream("", FileMode.Open))
            {
                //相关逻辑
            }
            //编译器会将其转化为：
            var fs1 = new FileStream("", FileMode.Open);
            try
            {
                //相关逻辑
            }
            finally
            {
                if (fs1 != null)
                {
                    ((IDisposable)fs1).Dispose();
                }
            }

            //标准销毁语义(非强制)
            //1.对象销毁后就无法恢复, 也不能重新激活
            //2.可以重复调用Display方法, 且不会报任何错
            //3.若可销毁对象x“拥有”可销毁对象y, 那么在对象x的Display方法会自动调用对象y的Display方法

            //销毁对象的时机
            //大多数情况当不使用对象时就可以销毁对象
            //但是有三种情况不适合销毁对象: 
            //1.当你并不持有该对象时, 例如某个全局静态对象(Db连接池)
            //2.当一个对象的Display方法执行了期待之外的操作时,
            //  例如IDbConnection如果需要重新打开数据库连接应该调用Close而不是Display(觉得这种本身就不属于不使用的对象)
            //3.当一个对象的Display方法在设计上不是必须的, 而且释放该对象会增加程序的复杂性时, 忽略对象的销毁功能

            //在销毁时清理字段
            //在销毁对象时最好清除对象自身的事件处理器(赋值为null)
        }

        #endregion

        #region 选择性销毁

        /// <summary>
        /// 选择性销毁
        /// </summary>
        [Test]
        public void SelectiveDisplayTest()
        {
            //当一个对象的Display方法中存在不必要的功能和必要的功能时,
            //进行部分不需要调用不必要方法的销毁时就会出现问题, 这时可以使用选择性销毁解决这个问题
            //例如DeflateStream就使用了这种模式
        }

        public sealed class HoseManager : IDisposable
        {
            public readonly bool checkMailOnDisplay;

            public HoseManager(bool checkMailOnDisplay)
            {
                this.checkMailOnDisplay = checkMailOnDisplay;
            }

            public void Dispose()
            {
                if (checkMailOnDisplay)
                {
                    CheckTheMail();
                }

                LockTheHouse();
            }

            //不必要方法
            public void CheckTheMail()
            {

            }

            //必要方法
            public void LockTheHouse()
            {

            }
        }

        #endregion

        #region 在终结器中调用Display方法

        /// <summary>
        /// 在终结器中调用Display方法
        /// </summary>
        [Test]
        public void DisplayInFinalizersTest()
        {
            //在终结器中调用Display方法是一种常用的模式, 这个模式通常在消费者忘记调用Dispose方法时作为补救
        }

        public class Test : IDisposable
        {
            ~Test()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);

                //为了防止垃圾回收器在之后回收这个对象时执行终结器
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    //在此实例拥有的其他对象上调用Dispose()。
                    //您可以在这里引用其他可终结的对象
                }

                //释放该对象拥有的非托管资源。
            }
        }

        #endregion
    }
}