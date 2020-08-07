using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace SampleCode.Test.StreamAndIO
{
    /// <summary>
    /// Stream的基本操作
    /// </summary>
    [TestFixture]
    public class StreamTest
    {
        /// <summary>
        /// Stream的基本操作
        /// </summary>
        [Test]
        public void StreamCodeTest()
        {
            //.Net流的架构主要包含三种概念: 后台存储、装饰器和流适配器
            //流可以分为后台存储流(FileStream、MemoryStream等)和装饰器流(DeflateStream、GZipStream、BufferStream等),
            //他们都是对Stream的包装, 即他们和Stream一样都仅支持字节操作
            //1.流适配器本身不是流, 是用于将一些数据类型转换为字节
            //2.后台存储流是与特定的后台存储类型连接的流, 负责处理原始数据
            //3.装饰器流会使用其他的流, 并以某种方式转换数据(如压缩、加密等)

            //Stream仅支持字节操作
            //抽象的Stream类是所有流的基类, 因此下面的例子都通过FileStream举例(抽象类无法实例化)

            //读取和写入
            //流可以支持读操作、写操作或两者都支持
            using (var s1 = new FileStream(@"D:\Desktop\10.18(1).json", FileMode.OpenOrCreate))
            {
                //可以通过CanWrite判断流是否可写, CanRead判断流是否可读
                Console.WriteLine(s1.CanWrite);     //true
                Console.WriteLine(s1.CanRead);      //true

                //读取
                //使用Read将流中的一个数据块读到一个字节数组中, 流仅会读1到10个字节的内容
                var data = new byte[10];
                s1.Read(data, 0, data.Length);
                var str = Encoding.UTF8.GetString(data);
                //使用ReadByte每次读取一个字节, 当流结束时会返回-1
                s1.ReadByte();

                //写入
                //使用Write或者WriteByte将数据发送到流
                var writeBytes = Encoding.UTF8.GetBytes("写入字节");
                s1.Write(writeBytes);
                s1.WriteByte(1);
            }

            //查找
            using (var s2 = new FileStream(@"D:\Desktop\10.18(1).json", FileMode.OpenOrCreate))
            {
                //如果CanSeek为true, 则流是可查找的
                Console.WriteLine(s2.CanSeek);  //true
                //可查找的流可以设置流的Length(SetLength方法), 也可以通过Position属性随时设置流的读写位置
                Console.WriteLine(s2.Length);
                s2.SetLength(s2.Length - 1);
                Console.WriteLine(s2.Length);
                //Seek方法参照当前位置或者结束位置进行位置的设置
                var l = s2.Seek(10, SeekOrigin.End);
            }

            //关闭和刷新
            //流在使用结束后必须销毁, 以释放底层资源
            //Dispose和Close方法的功能时一致的
            //重新销毁或者关闭流不会产生任何错误
            //关闭装饰器流时, 会同时关闭其后台存储流
            //当关闭流或者销毁流时会自动调用Flush方法

            //超时
            //如果流的CanTimeOut属性为true, 那么就可以为这个流设置读写超时时间
        }
    }
}