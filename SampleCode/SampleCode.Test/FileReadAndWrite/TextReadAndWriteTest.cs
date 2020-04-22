using NUnit.Framework;
using NUnit.Framework.Internal;
using SampleCode.FileReadAndWrite;

namespace SampleCode.Test.FileReadAndWrite
{
    [TestFixture]
    public class TextReadAndWriteTest
    {
        private static TextReadAndWrite model = new TextReadAndWrite();

        [Test]
        public void TextWriteToFileStreamTest()
        {
            model.TextWriteToFileStream(@"D:\Desktop\1.txt", "hhhh");
        }

        [Test]
        public void TextReadToFileStreamTest()
        {
            model.TextReadToFileStream(@"D:\Desktop\1.txt");
        }

        [Test]
        public void TextWriteToStreamWriterTest()
        {
            model.TextWriteToStreamWriter(@"D:\Desktop\1.txt", "xxxx");
        }

        [Test]
        public void TextReadToStreamReaderTest()
        {
            model.TextReadToStreamReader(@"D:\Desktop\1.txt");
        }

        [Test]
        public void TextWriteToFileTest()
        {
            model.TextWriteToFile(@"D:\Desktop\1.txt", "qqqq");
        }

        [Test]
        public void TextReadToFileTest()
        {
            model.TextReadToFile(@"D:\Desktop\1.txt");
        }
    }
}