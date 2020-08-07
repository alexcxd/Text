using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SampleCode.Test.StreamAndIO
{
    /// <summary>
    /// 装饰器流
    /// </summary>
    [TestFixture]
    public class DecoratorStreamTest
    {
        #region BufferedStream基本操作

        /// <summary>
        /// BufferedStream基本操作
        /// </summary>
        [Test]
        public void BufferedStreamTest()
        {
            //BufferedStream可以装饰另一个具有缓冲功能的流
            //缓冲可以减少后台存储的回程调用从而提高性能
            File.WriteAllBytes("myFile.bin", new byte[1000000]);
            //下例会提前将fs的数据读取到bs中, 因此第一次虽然只读了一个字节,
            //但是底层流实际上已经向前读取了20000字节,即fs的Position属性已经为20000了
            using (var fs = new FileStream("myFile.bin", FileMode.Open, FileAccess.Read))
            using (var bs = new BufferedStream(fs, 20000))
            {
                bs.ReadByte();
                Console.WriteLine(fs.Position);
            }

        }

        #endregion

        #region 压缩流

        /// <summary>
        /// 压缩流
        /// </summary>
        [Test]
        public async Task ZipStreamTest()
        {
            //两个通用的压缩流: GZipStream, DeflateStream
            //两者的区别在于GZipStream会在开头和结尾写入额外的协议信息, 其中包括检测错误CRC
            //它们的构造函数接受底层流参数, 并将数据压缩写入底层流或者从底层流读取数据并解压

            #region 文件数据压缩

            //高密度的非重复的二进制文件压缩效果很差(缺少设计规范的加密数据的压缩比是最差的)

            //高密度的非重复的二进制文件的压缩, 压缩后的文件大小和压缩前的文件大小几乎不变
            //压缩
            using (var sCompress = File.Create("ZipStreamTestCompress.bin"))
            using (var sNoCompress = File.Create("ZipStreamTestNoCompress.bin"))
            using (var dsCompress = new DeflateStream(sCompress, CompressionMode.Compress))
            using (var dsNoCompress = new DeflateStream(sNoCompress, CompressionMode.Compress))
            {
                for (byte i = 0; i < 100; i++)
                {
                    dsCompress.WriteByte(i);
                    dsNoCompress.WriteByte(i);
                }
            }
            //解压
            using (var s = File.OpenRead("ZipStreamTestCompress.bin"))
            using (var ds = new DeflateStream(s, CompressionMode.Decompress))
            {
                for (byte i = 0; i < 100; i++)
                {
                    var b = ds.ReadByte();
                }
            }

            //高重复的二进制文件的压缩, 由5KB压缩至1KB
            //下例从一个简短的句子中随机抽取1000个单词形成文本流, 并对该文本流进行压缩
            string[] words = "The quick brown fox jumps over the lazy dog".Split(" ");
            var rand = new Random();

            using (var sCompress = File.Create("ZipStreamTestCompress.bin"))
            using (var sNoCompress = File.Create("ZipStreamTestNoCompress.bin"))
            using (var dsCompress = new DeflateStream(sCompress, CompressionMode.Compress))
            using (var writerCompress = new StreamWriter(dsCompress))
            using (var writerNoCompress = new StreamWriter(sNoCompress))
            {
                for (int i = 0; i < 1000; i++)
                {
                    await writerCompress.WriteAsync(words[rand.Next(words.Length)] + " ");
                    await writerNoCompress.WriteAsync(words[rand.Next(words.Length)] + " ");
                }
            }

            #endregion

            #region 内存数据压缩

            //DeflateStream的Dispose是非常标准的关闭流的方法, 它会清理该过程中所有未写入的缓存, 也会关闭对应的底层流
            //如果不希望它关闭对应的底层流, 可以通过向Deflate的构造器中传入额外的标记(leaveOpen)实现

            var bytes = new byte[1000];
            var noCompressionLength = bytes.Length; //1000

            //压缩
            var ms = new MemoryStream();
            using (var ds = new DeflateStream(ms, CompressionMode.Compress))
            //using (var ds = new DeflateStream(ms, CompressionMode.Compress, true))    //不关闭底层流的写法
            {
                ds.Write(bytes, 0, bytes.Length);
            }

            var bytesCompress = ms.ToArray();
            var compressionLength = bytesCompress.Length;   //11

            //解压
            ms = new MemoryStream(bytesCompress);
            using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
            {
                for (var i = 0; i < 1000; i += ds.Read(bytes, i, 1000 - i));
            }

            #endregion
        }

        #endregion
    }
}