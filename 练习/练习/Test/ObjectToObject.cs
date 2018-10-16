using System;
using System.Reflection;

namespace 练习.Test
{
    public class ObjectToObject
    {
        public void Test()
        {
            Test2 test2 = new Test2()
            {
                Str1 = "1",
                Str2 = "2",
                Str3 = "3",
                Str4 = "4",
                Str5 = "5",
                Para = 1,
                Time1 = DateTime.Parse("1900/01/01")
            };

            Test1 test1 = new Test1()
            {
                Str1 = "11",
                Str2 = "22",
                Str3 = "33",
                Para = "1",
                Time1 = DateTime.Now
            };

            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            FieldInfo[] fieldsTest1 = typeof(Test1).GetFields(flag);
            FieldInfo[] fieldsTest2 = typeof(Test2).GetFields(flag);

            //部分映射，不覆盖没有部分
            for (int i = 0; i < fieldsTest1.Length; i++)
            {
                for (int j = 0; j < fieldsTest2.Length; j++)
                {
                    if (fieldsTest1[i].Name.Equals(fieldsTest2[j].Name) &&
                        fieldsTest1[i].FieldType == fieldsTest2[j].FieldType)
                    {
                        fieldsTest2[j].SetValue(test2, fieldsTest1[i].GetValue(test1));
                        break;
                    }
                }
            }
        }
    }

    public class Test1
    {
        public string Str1 { get; set; }
        public string Str2 { get; set; }
        public string Str3 { get; set; }
        public string Para { get; set; }
        public DateTime Time1 { get; set; }
    }

    public class Test2
    {
        public string Str1 { get; set; }
        public string Str2 { get; set; }
        public string Str3 { get; set; }
        public string Str4{ get; set; }
        public string Str5 { get; set; }
        public int Para { get; set; }
        public DateTime Time1 { get; set; }
    }
}