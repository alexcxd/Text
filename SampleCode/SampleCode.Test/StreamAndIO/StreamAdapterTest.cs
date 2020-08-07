using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace SampleCode.Test.StreamAndIO
{
    /// <summary>
    /// 流适配器
    /// </summary>
    [TestFixture]
    public class StreamAdapterTest
    {
        #region 文本适配器

        /// <summary>
        /// 文本适配器
        /// </summary>
        [Test]
        public void TextStreamTest()
        {
            //TextReader和TextWriter都是专门用于处理字符和字符串的适配器, 它们是抽象类
            //通用实现: 1.StreamReader/StreamWriter: 使用Stream存储其原始数据, 将流的字节转换为字符或者字符串
            //          2.StringReader/StreamWriter: 使用内存字符串实现TextReader/TextWriter(不包装流)

            //1.Reader/StreamWriter
            //使用StreamWriter写入文件, 既可以使用StreamWriter直接实例化, 也可以使用File的静态函数
            using (var fs = new FileStream("TextStreamTestToStream.txt", FileMode.Create, FileAccess.Write))
            using (var sw = new StreamWriter(fs))
            //using (var sw = File.CreateText("TextStreamTest.txt"))
            {
                sw.WriteLine("Line1");
                sw.WriteLine("Line2");
            }
            //使用StreamReader读文件
            using (var fs = new FileStream("TextStreamTestToStream.txt", FileMode.Open, FileAccess.Read))
            using (var sr = new StreamReader(fs))
            //using (var sr = File.OpenText("TextStreamTest.txt"))
            {
                //两种判断sr读取到文件结尾的方法
                //1.StreamReader.Peek()的返回值小于-1时
                //2.StreamReader.ReadLine返回null时
                while (sr.Peek() > -1)
                {
                    Console.WriteLine(sr.ReadLine());
                }
            }
            //字符编码
            //在创建StreamReader/StreamWriter时需要选定一种编码方式, 默认UTF-8
            //默认情况下, StreamWriter在指定编码时会在流的起始部分写入一个前缀来识别编码, 但通常这不是一种好做法
            //可以通过在实例化编码时将encoderShouldEmitUTF8Identifier置为false来取消在流的起始位置写去前缀,
            //throwOnInvalidBytes置为true可以在编码无法转换为有效字符串时抛出异常
            var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

            //2.StringReader/StringWriter
            var sb = new StringBuilder("");
            using (var sw = new StringWriter(sb))
            {
                sw.WriteLine("Line1");
                sw.WriteLine("Line2");
            }
            using (var sr = new StringReader(sb.ToString()))
            {
                while (sr.Peek() > -1)
                {
                    Console.WriteLine(sr.ReadLine());
                }
            }
        }

        #endregion

        #region 二进制适配器

        /// <summary>
        /// 二进制适配器
        /// </summary>
        [Test]
        public void BinaryStreamTest()
        {
            //BinaryReader和BinaryWriter能够读写基本的数据类型
            var person = new Person("Fragment", 23, 1.78, "hh");
            //使用BinaryWriter写入文件
            using (var fs = new FileStream("BinaryStreamTest.txt", FileMode.Create, FileAccess.Write))
            {
                person.SaveData(fs);
            }
            person.name = "1";
            //使用BinaryReader读文件
            using (var fs = new FileStream("BinaryStreamTest.txt", FileMode.Open, FileAccess.Read))
            {
                person.LoadData(fs); 
            }
            //一次性读取所有字节
            using (var fs = new FileStream("BinaryStreamTest.txt", FileMode.Open, FileAccess.Read))
            {
                var data = new BinaryReader(fs).ReadBytes((int)fs.Length);
                //var str = Encoding.UTF8.GetString(data);
            }
        }

        public class Person
        {
            public string name;
            public int age;
            public double height;
            public string nickName;

            public Person(string name, int age, double height, string nickName)
            {
                this.name = name;
                this.age = age;
                this.height = height;
                this.nickName = nickName;
            }

            public void SaveData(Stream s)
            {
                var w = new BinaryWriter(s);
                w.Write(name);
                w.Write(age);
                w.Write(height);
                w.Write(nickName);
                w.Flush();
            }

            public void LoadData(Stream s)
            {
                //在读取BinaryWriter写入的数据是, 读取顺序需要和写入顺序一致
                var r = new BinaryReader(s);
                name = r.ReadString();
                age = r.ReadInt32();
                height = r.ReadDouble();
                nickName = r.ReadString();
            }
        }

        #endregion

        #region 关闭和销毁流适配器

        /// <summary>
        /// 关闭和销毁流适配器
        /// </summary>
        [Test]
        public void CloseOrDisplayStreamAdpterTest()
        {
            //流适配器销毁的4种方式
            //1.只关闭适配器
            //2.关闭适配器, 而后关闭流
            //3.(对于写入器)先刷新适配器, 而后关闭流
            //4.(对于读取器)直接关闭流‘

            //方法1和方法2语义上是相同的, 因为关闭适配器会自动关闭其底层流
            //在Framework 4.5开始, 增加了StreamReader/StreamWriter增加了一个构造函数, 保证流在适配器关闭后仍然打开
            using (var fs = new FileStream("CloseOrDisplayStreamAdpterTest.txt", FileMode.Create))
            {
                //第一个参数为需要写入的流, 第二参数为字符编码, 第三个参数为缓冲池大小, 第四个参数为在关闭时是否关闭底层流
                using (var sw = new StreamWriter(fs, new UTF8Encoding(false, true), 0x400, false)) { }

                fs.Position = 0;
                var b = fs.ReadByte();
            }

            //一定不要在关闭或者刷新一个适配器之前,关闭流, 那样会导致适配器所有的缓存数据都丢失
        }

        #endregion
    }
}