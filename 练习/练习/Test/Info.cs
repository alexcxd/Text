using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace 练习.Test
{
    public class Info
    {   
        /// <summary>
        /// 获取注释和属性的键值关系对
        /// </summary>
        public void LoadFile()
        {
            string path = @"C:\Users\Administrator\Desktop\cxd\trunk\XStore\XStore.Entity\MainEntity\Mall.cs";
            if (!File.Exists(path))
            {
                Console.WriteLine("文件不存在");
            }
            List<KeyValue> keyValueList = new List<KeyValue>();
            StreamReader fs = new StreamReader(path);
            string isCs = fs.ReadToEnd();
            //可以尝试直接在一个正则里将两个值取出来
            var list = Regex.Matches(isCs, @"<summary>(?<text>[^>]+)</summary>[^{]*public(?<property>[^}]+){");
            foreach (Match match in list)
            {
                KeyValue value = new KeyValue();
                string text = match.Groups["text"].Value;
                string annotation = text.Split('/')[3].Trim();
                string text1 = match.Groups["property"].Value.Trim();
                string property = text1.Split(' ')[1];
                //string property = Regex.Match(text1, "public(?<property>[^}]+){").Groups["property"].Value.Trim();
                value.Annotation = annotation;
                value.Property = property;
                keyValueList.Add(value);
            }
        }
    }
}