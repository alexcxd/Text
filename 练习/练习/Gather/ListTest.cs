using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Gather
{
    class ListTest
    {
        //正常集合
        public static void ListTest1()
        {
            //列表初始化容量为4个元素，每次超出容量都会翻倍
            //当容量发生改变时，整个集合就要重新分配到一个新的内存块中
            

            //设置初始容量
            List<int> intList = new List<int>(10);
            intList.Capacity = 20;

            //设置初始值
            List<int> intList2 = new List<int>() { 1, 2 };
            List<string> strList = new List<string>() { "1", "2" };

            List<Racer> racers = new List<Racer>()
            {
                new Racer(14, "Niki", "Lauda", "Austria", 25),
                new Racer(21, "Ayrton", "Prost", "France", 51),
            };
            //添加单个元素
            racers.Add(new Racer(20, "Alex", "WenZhou", "China", 1));

            //添加多个元素
            racers.AddRange(new Racer[]
            {
                new Racer(12, "Jochen", "Rindt", "Austria", 6),
                new Racer(22, "Ayrton", "Senna", "Brazil", 41)
            });

            //插入元素
            racers.Insert(3, new Racer(6, "Phil", "Hill", "USA", 3));

            //插入大量元素
            racers.InsertRange(1,new Racer[]{ });

            //通过委托，对列表里的每个元素进行某种操作，参数为方法名
            racers.ForEach(Console.WriteLine);

            //删除指定位置的元素
            racers.RemoveAt(1);

            //删除某个区间的元素
            racers.RemoveRange(1, 2);

            //搜索某一对象返回索引值
            var racer = new Racer(22, "Ayrton", "Senna", "Brazil", 41);
            var num = racers.IndexOf(racer);

            //通过委托和Lambda表达式查找，返回查找元素的索引,如果有多个，返回第一个
            var racer1 =  racers.FindIndex(m => m.Country == "China");

            //通过委托和Lambda表达式查找，返回查找元素,如果有多个，返回第一个
            var racer2 = racers.Find(m => m.Country == "China");

            //通过委托和Lambda表达式查找，返回查找元素列表
            var racerList = racers.FindAll(m => m.Country == "China");

            //排序,只有集合中的类型实现了IComparable接口才能使用不带参数的sort
            racers.Sort();

            //若集合中的类型未实现Icomparable接口，需传递一个是了该接口的对象
            racers.Sort(new RacerComparer(RacerComparer.ComparerType.Wins));

            //通过委托和Lamdbda表达式结合排序
            racers.Sort((m, n) => m.LastName.CompareTo(n.LastName));

            //类型转换
            var personList = racers.ConvertAll(m => new Person(m.FristName + " " + m.LastName));

            //只读集合,所有修改集合的方法和属 性都抛出NotsuppoHedException异常。
            var onlyReadList = racers.AsReadOnly();
        }

       
            

    }

    public class RacerComparer : IComparer<Racer>
    {
        public enum ComparerType
        {
            FristName,
            LastName,
            Country,
            Wins
        }

        private ComparerType comarerType;

        public RacerComparer(ComparerType ComparerType)
        {
            this.comarerType = ComparerType;
        }

        public int Compare(Racer x, Racer y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if(y == null)
            {
                throw new ArgumentNullException("y");
            }

            int result;

            switch (comarerType)
            {
                case ComparerType.FristName:
                    return x.FristName.CompareTo(y.FristName);
                    break;
                case ComparerType.LastName:
                    return x.LastName.CompareTo(y.LastName);
                    break;
                case ComparerType.Country:
                    //如果国家相同，就根据名字排序
                    if((result = x.Country.CompareTo(y.Country)) == 0)
                    {
                        return x.FristName.CompareTo(y.FristName);
                    }
                    else
                    {
                        return x.Country.CompareTo(y.Country);
                    }
                    break;
                case ComparerType.Wins:
                    return x.Wins.CompareTo(y.Wins);
                    break;
                default:
                    throw new ArgumentException("Invalid Compara Type");
            }

        }
    }

    public class Racer : IComparable<Racer>, IFormattable
    {
        public int Id { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public int Wins { get; set; }

        public Racer(int Id, string FristName, string LastName, string Country, int Wins)
        {
            this.Id = Id;
            this.FristName = FristName;
            this.LastName = LastName;
            this.Country = Country;
            this.Wins = Wins;
        }
            
        public int CompareTo(Racer other)
        {
            int compare = this.LastName.CompareTo(other.LastName);
            if(compare == 0)
            {
                return this.FristName.CompareTo(other.FristName);
            }

            return compare;
        }

        public override string ToString()
        {
            return String.Format($"{0} {1}", FristName, LastName);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if(format == null)
            {
                format = "N";
            }

            switch (format.ToUpper())
            {
                case "N":
                    return ToString();
                case "F":
                    return FristName;
                case "L":
                    return LastName;
                case "W":
                    return String.Format($"{0} Wins:{1}",ToString(), Wins);
                case "C":
                    return String.Format($"{0} Country:{1}", ToString(), Country); ;
                case "A":
                    return String.Format($"{0} {1} Wins:{2}", ToString(), Country, Wins); ;
                default:
                    throw new FormatException(String.Format(formatProvider, $"Format {0} is not supported", format));
            }
        }
    }

    public class Person
    {
        private string name;

        public Person(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
