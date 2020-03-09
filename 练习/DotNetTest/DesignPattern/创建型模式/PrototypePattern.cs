using System;
using Newtonsoft.Json;

namespace DotNetTest.DesignPattern.创建型模式
{
    /// <summary>
    /// 原型模式
    /// </summary>
    /// 用原型实例指定创建对象的种类，并通过拷贝这些原型创建新的对象
    /// 减少实例所消耗的资源
    class PrototypePattern
    {
        public static void PrototypePatternMain()
        {
            
            Person person = new Person("cxd", 22, new Remark("yu"));

            //ICloneable
            /*var personClone = person.Clone() as Person;
            personClone.Name = "alex";
            personClone.Age = 21;
            personClone.Remark.Text = "1";*/

            //用反射进行克隆 浅复制
            var personReflect = Activator.CreateInstance<Person>();
            personReflect.Name = person.Name;
            personReflect.Age = person.Age;
            personReflect.Remark = person.Remark;

            //序s列化再反序列化 深复制 动态的但是速度比反射还慢
            var person1 = JsonConvert.DeserializeObject<Person>(JsonConvert.SerializeObject(person));
            person1.Name = "alex";
            person1.Age = 21;
            person1.Remark.Text = "1";

            //使用IL进行克隆


            //使用扩展方法进行克隆
        }
    }


    public class Person : ICloneable
    {
        public Person() { }
        public Person(string name, int age, Remark remark)
        {
            this.Name = name;
            this.Age = age;
            this.Remark = remark;
        }

        public string Name { get; set; }
        public int Age { get; set; }
        public Remark Remark { get; set; }

        public object Clone()
        {
            //浅复制
            //return this.MemberwiseClone();


            //深复制
            Person person = new Person();
            person.Name = this.Name;
            person.Age = this.Age;
            //深复制
            if (Remark != null)
            {
                person.Remark = this.Remark.Clone() as Remark;
            }
            return person;
        }
    }

    public class Remark : ICloneable
    {
        public Remark() { }
        public Remark(string text)
        {
            this.Text = text;
        }

        public string Text { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
