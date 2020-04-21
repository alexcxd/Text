using DotNetTest.DesignPattern.创建型模式;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleCode.Test.DesignPattern
{
    public class PrototypePatternTest
    {
        /// <summary>
        /// 原型模式
        /// </summary>
        [Test]
        public void PrototypePatternCodeTest()
        {
            Person person = new Person("cxd", 22, new Remark("yu"));

            //ICloneable
            var personClone = person.Clone() as Person;
            personClone.Name = "alex";
            personClone.Age = 21;
            personClone.Remark.Text = "1";

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
}
