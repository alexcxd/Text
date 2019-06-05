using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTest.ClassTest
{
    public class StringTest
    {
        public void StringFormatTest(double strs)
        {
            //｛1,2:3｝第一个参数是标识，第二个参数正数为右对齐，负值为左对齐；第三个参数为格式类型
            string str = String.Format("{0,0:E}", strs);
            string str1 = String.Format("{0,4:F}", strs);
            string str2 = String.Format("{0,0:G}", strs);
            string str3 = String.Format("{0,0:N}", strs);
            string str4 = String.Format("{0,0:P}", strs);
            Console.WriteLine(str);
        }

    }
}
