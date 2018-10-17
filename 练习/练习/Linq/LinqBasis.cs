using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test.Linq
{
    public class LinqBasis
    {
        /// <summary>
        /// 分组
        /// </summary>
        public void GroupByTest()
        {
            //Linq GroupBy测试
            /*var peoples = from m in Person.GetPeopleList()
                          group m by m.Country into g
                          orderby g.Count()
                          select new
                          {
                              Country = g.Key,
                              Count = g.Count()
                          };*/

            //Linq中的GroupBy会返回一个GroupedEnumerable对象，存了n个LookUp对象
            var peoples2 = Person.GetPeopleList().
                GroupBy(r => r.Country);

            foreach (var g in peoples2)
            {
                var key = g.Key;
                foreach (var item in g)
                {
                    var firstName = item.FirstName;
                    var Age = item.Age;
                }
            }

        }

        /// <summary>
        /// 合并
        /// </summary>
        public void ZipTest()
        {
            var peoplesFirst = from m in Person.GetPeopleList()
                               orderby m.Id
                               select new
                               {
                                   Country = m.Country
                               };
            var peoplesSecond = from m in Person.GetPeopleList()
                                orderby m.Id
                                select new
                                {
                                    Name = m.FirstName + " " + m.LastName
                                };

            var peoplesZip = peoplesFirst.Zip(peoplesSecond,
                (m, n) => n.Name + " |Country: " + m.Country);

        }

        /// <summary>
        /// 聚合函数
        /// </summary>
        public void AggregateFunctionTest()
        {
            //Count 返回集合中的项数
            //Sum 计算集合中值的和
            //Min 得到集合中最小值
            //Max 得到集合中最大值
            //Average 计算集合中值的平均数
            //Aggregate 可以做一些复杂的聚合运算 它接受2个参数，
            //一般第一个参数是称为累积数（默认情况下等于第一个值），而第二个代表了下一个值。

            var peoplesFirst = Person.GetPeopleList().
                Select(m => m.FirstName + " " + m.LastName).
                Aggregate((all, next) => (all + " | " + next));
                                
            int[] arrInt = { 1, 2, 3, 4, 56, 23 };
            var result = arrInt.Aggregate((all, next) => (all + next));
        }

        /// <summary>
        /// 并行Linq和表达式树
        /// </summary>
        public void ParalleTest()
        {
            int[] data = Common.GetIntList();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            //AsParallel使当前Linq并行于多个CPU上运行，可以提高运算速度(1亿条数据优化了约40%)
            var sum = (from x in data.AsParallel()
                       where x < 20
                       select x).Sum();

            sw.Stop();
            var time = sw.ElapsedMilliseconds;

            //表达式树
            //UnaryExpression; //一元运算表达式
            //BinaryExpression; //二元运算表达式
            //ConstantExpression; //常量表达式
            //ParameterExpression; //变量、变量参数表达式
            //GotoExpression; //跳转语句表达式，如：return。continue、break
            //BlockExpression; //块语句表达式
            //ConditionalExpression; //条件语句表达式
            //LoopExpression;  //循环语句表达式
            //SwitchExpression; //选择语句表达式
            //IndexExpression; //访问数组索引表达式
            //MethodCallExpression; //调用方法表达式
            //LambdaExpression;  //Lambda表达式
            //TypeBinaryExpression; //类型检查表达式
            //NewArrayExpression;  // 创建数组表达式
            //DefaultExpression; //默认值表达式
            //DynamicExpression; //动态类型表达式
            //TryExpression; //try语句表达式
            //MemberExpression; //类成员表达式
            //InvocationExpression; //执行Lambda并传递实参的表达式
            //NewExpression; //调用无参构造函数表达式
            //MemberInitExpression; //调用带参构造函数表达式，可初始化成员
            //ListInitExpression; //集合初始化器表达式
            Expression<Func<Person, bool>> expression = r => r.Country == "China" && (r.Age > 20 && r.FirstName == "FirstName1");
            LambdaExpression lambdaExpression = (LambdaExpression)expression;

            var body = lambdaExpression.Body;
            var right = body.NodeType;

            foreach (var parameter in lambdaExpression.Parameters)
            {

            }

        }

    }

}

public class Common
{
    const int arraySize = 100000000;

    public static int[] GetIntList()
    {
        var data = new int[arraySize];
        var r = new Random();
        for (int i = 0; i < arraySize; ++i)
        {
            data[i] = r.Next(40);
        }

        return data;
    }
}

public class Person
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 姓
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// 名
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// 国籍
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// 年龄
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// 成功次数
    /// </summary>
    public int SucceedNum { get; set; }

    /// <summary>
    /// 获取Person集合
    /// </summary>
    /// <returns></returns>
    public static List<Person> GetPeopleList()
    {
        List<Person> list = new List<Person>
            {
                new Person()
                {
                    Id = 1,
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Country = "China",
                    Age = 20,
                    SucceedNum = 3
                },
                new Person()
                {
                    Id = 2,
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Country = "China",
                    Age = 8,
                    SucceedNum = 6
                },
                new Person()
                {
                    Id = 3,
                    FirstName = "FirstName3",
                    LastName = "LastName3",
                    Country = "Japan",
                    Age = 31,
                    SucceedNum =8
                },
                new Person()
                {
                    Id = 4,
                    FirstName = "FirstName4",
                    LastName = "LastName4",
                    Country = "Japan",
                    Age = 40,
                    SucceedNum =17
                },
            };

        return list;
    }
}

