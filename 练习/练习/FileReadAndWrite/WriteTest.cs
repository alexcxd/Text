using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 练习.FileReadAndWrite
{
    class WriteTest
    {
        public static void Demo1()
        {
            string filePath = @"E:\Desktop\漫画\1.txt";
           
            byte[] data = System.Text.Encoding.Default.GetBytes("Hello World!");
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(data, 0, data.Length);
            }            
        }

        public static void Demo2()
        {

        }

    }
}
