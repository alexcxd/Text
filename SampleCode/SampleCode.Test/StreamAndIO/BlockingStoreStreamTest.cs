using System.IO;
using NUnit.Framework;

namespace SampleCode.Test.StreamAndIO
{
    /// <summary>
    /// 后台存储流
    /// </summary>
    [TestFixture]
    public class BlockingStoreStreamTest
    {
        /// <summary>
        /// FileStream基本操作
        /// </summary>
        [Test]
        public void FileStreamTest()
        {
            //实例化FileStream的方法
            //1.通过File类型中的静态方法进行实例
            using (var fsOpenRead = File.OpenRead(@"D:\Desktop\10.18(1).json")) { }     //打开读(保留原来的文件, 且流起始位置为0)
            using (var fsWrite = File.OpenWrite(@"D:\Desktop\10.18(1).json")) { }       //打开写(保留原来的文件, 且流起始位置为0)
            using (var fsCreate = File.Create(@"D:\Desktop\10.18(1).json")) { }         //创建(会删除原来的文件)
            //2.直接实例化FileStream
            //  a. 文件地址, 可以是绝对路径也可以是相对路径(必须)
            //  b. FileMode, 用于指定操作系统打开文件的方式(必须)
            //  c. FileAccess, 可以确定流的读写模式, 默认ReadWrite
            //  d. FileShare, 确定在当前流文件处理完成前, 若其他进程需要访问文件时所拥有的权限, 默认Read
            //  e. 内部缓存池大小, 单位为字节, 默认4KB
            //  f. 是否由操作系统管理异步I/O
            //  g. FileOptions, 它包括请求操作系统加密(Encrypted), 文件关闭时自动删除临时文件(DeleteOnClose),
            //                  优化提示(RandomAccess和SequentialScan), 禁用写后缓存(WriteThrough)
            using (var fs1 = new FileStream(@"D:\Desktop\10.18(1).json", FileMode.Open, FileAccess.Read, FileShare.Read,
                10, FileOptions.Asynchronous)) { }
        }

        /// <summary>
        /// MemoryStream基本操作
        /// </summary>
        [Test]
        public void MemoryStreamTest()
        {
            //MemoryStream使用数组作为后台存储, 这个后台存储是必须一次性全部写入内存的
            //可以通过CopyTo将一个大小可以承受的流复制到MemoryStream中
            var fsOpenRead = File.OpenRead(@"D:\Desktop\10.18(1).json");
            var memoryStream = new MemoryStream();
            fsOpenRead.CopyTo(memoryStream);
            //可以通过调用ToArray将一个MemoryStream转换为一个字节数组(即使关闭了MemoryStream, 也可以使用ToArray获取底层数据)
            var bytes1 = memoryStream.ToArray();
            //可以通过GetBuffer方法高效的直接返回底层存储数组的引用
            var bytes2 = memoryStream.GetBuffer();
        }
    }
}