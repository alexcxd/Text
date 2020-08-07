using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using NUnit.Framework;

namespace SampleCode.Test.StreamAndIO
{
    /// <summary>
    /// 文件与目录操作
    /// </summary>
    [TestFixture]
    public class FileTest
    {
        #region 文件

        /// <summary>
        /// 文件基本操作
        /// </summary>
        [Test]
        public void FileCodeTest()
        {
            //File是操作文件的静态类, FileInfo是操作文件的实例方法类
            var filePath = @"FileCodeTest.txt";

            try
            {
                //文件的属性调整
                //在不影响其他属性的情况下, 替换文件的其中一个选项(只读)
                File.Create(filePath);
                //使用静态类File
                var fa = File.GetAttributes(filePath);
                if ((fa & FileAttributes.ReadOnly) != 0)
                {
                    fa ^= FileAttributes.ReadOnly;
                    File.SetAttributes(filePath, fa);
                }
                //使用实例化方法类FileInfo
                var fileInfo = new FileInfo(filePath) { IsReadOnly = false };

                //压缩和加密
                //对于加密文件可以调用静态类File的Encrypt方法和Decrypt方法

                //文件的安全性
                //可以通过FileSecurity对象查询或修改操作系统授予用户和角色的权限
                //创建一个新文件时也可以通过给FileStream的构造器传递一个FileSecurity来指定文件的权限
                var sec = new FileSecurity(filePath, AccessControlSections.Access);
                var rules = sec.GetAccessRules(true, true, typeof(NTAccount));
                foreach (FileSystemAccessRule rule in rules)
                {
                    var accessControlType = rule.AccessControlType;
                    var fileSystemRights = rule.FileSystemRights;
                    var identityReference = rule.IdentityReference.Value;
                }


            }
            finally
            {
                File.Delete(filePath);
            }
        }

        #endregion

        #region 目录

        /// <summary>
        /// 目录基本操作
        /// </summary>
        [Test]
        public void DirectoryTest()
        {
            //Directory是操作文件的静态类, DirectoryInfo是操作文件的实例方法类
            var directoryPath = @"D:\Desktop\cxd";

            //Directory
            //获取目录下所有文件(Get***和Enumerate***都是类似的)
            //GetFiles和EnumerateFiles均可以接受一个searchPattern和searchOption的重载方法,
            //若指定searchOption.AllDirectories, 会递归的进行子目录搜索
            //方法一 GetFiles
            var files1 = Directory.GetFiles(directoryPath, "", SearchOption.AllDirectories);
            //方法二 EnumerateFiles 效率相较于方法一高, 原因在于它是延迟加载的
            var files2 = Directory.EnumerateFiles(directoryPath);

            //DirectoryInfo
            var directoryInfo = new DirectoryInfo(directoryPath);
            var files3 = directoryInfo.GetFiles("", SearchOption.AllDirectories);
            var files4 = directoryInfo.EnumerateFiles();
        }

        #endregion

        #region Zip文件

        [Test]
        public void ZipFileTest()
        {
            //可以使用System.IO.Compression命名空间下的ZipArchive和ZipFile来操作Zip压缩格式的文件
            //ZipArchive可以操作流, ZipFile则可以执行文件操作

            //ZipFile
            //将指定文件夹压缩
            ZipFile.CreateFromDirectory(@"D:\Desktop\常用sql", @"D:\Desktop\常用sql.zip");
            //将一个压缩文件解压到指定文件夹
            ZipFile.ExtractToDirectory(@"D:\Desktop\cxd\mall-footer-cart0.zip", @"D:\Desktop");

            //ZipArchive
            //ZipArchive可以直接通过构造器实例化, 也可以使用ZipFile.Open实例化
            using (var zipArchive = ZipFile.Open(@"D:\Desktop\cxd\mall-footer-cart0.zip", ZipArchiveMode.Read))
            {
                //遍历Zip文件内所有项目
                foreach (var zipArchiveEntry in zipArchive.Entries)
                {
                    var name = zipArchiveEntry.FullName + " " + zipArchiveEntry.Length;
                }
            }
            using (var zipArchive = ZipFile.Open(@"D:\Desktop\cxd\mall-footer-cart0.zip", ZipArchiveMode.Update))
            {
                var fileName = "text.txt";

                //向Zip文件中创建一个新项目
                var data = Encoding.UTF8.GetBytes("zipTest");
                zipArchive.CreateEntry(fileName).Open().Write(data, 0, data.Length);

                //删除一个项目
                zipArchive.Entries.FirstOrDefault(x => x.Name == fileName)?.Delete();
            }
        }

        #endregion
    }
}