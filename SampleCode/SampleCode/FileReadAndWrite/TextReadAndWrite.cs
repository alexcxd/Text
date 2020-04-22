using System;
using System.IO;
using System.Text;

namespace SampleCode.FileReadAndWrite
{
    public class TextReadAndWrite
    {
        public void TextWriteToFileStream(string filePath, string text)
        {
            var data = System.Text.Encoding.Default.GetBytes(text);

            //using (var fs = File.OpenWrite(filePath))
            using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write,
                FileShare.Write))
            {
                fs.Write(data, 0, data.Length);
            }
        }

        public void TextReadToFileStream(string filePath)
        {
            //using (var fs = File.OpenRead(filePath))
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                var bytes = new byte[200];
                fs.Read(bytes);

                var str = Encoding.UTF8.GetString(bytes);

                Console.WriteLine(str);
            }
        }

        public void TextWriteToStreamWriter(string filePath, string text)
        {
            using (var sw = new StreamWriter(filePath))
            {
                sw.WriteLine(text);
            }
        }

        public void TextReadToStreamReader(string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                string line;

                // 从文件读取并显示行，直到文件的末尾 
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        public void TextWriteToFile(string filePath, string text)
        {
            File.WriteAllText(filePath, text);
        }

        public void TextReadToFile(string filePath)
        {
            var str = File.ReadAllText(filePath);
            Console.WriteLine(str);
        }
    }
}