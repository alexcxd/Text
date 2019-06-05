using System;
using System.Diagnostics;
using System.Text;

namespace DotNetTest.PerformanceTesting
{
    public class StringTest
    {
        /*
         * 拼接字符串性能测试（单位为毫秒）
         * 方式               10万      1万      100    
         * str += str         23356     92       0     
         * StringBuilder      17        0        0
         * String.Format      24277     105      0
         */
        public static void StringSplicing()
        {
            var sw = new Stopwatch();
            sw.Start();

            var str = "";
            for (int i = 0; i < 100; i++)
            {
                str += $"{"aaaaaaa"}";
            }

            sw.Stop();
            var time = sw.ElapsedMilliseconds;

        }
    }
}