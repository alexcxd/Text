using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace SampleCode.Test.Reflect
{
    /// <summary>
    /// 反射相关
    /// </summary>
    public class ReflectTest
    {
        #region 获取类型

        /// <summary>
        /// 获取类型
        /// </summary>
        [Test]
        public void TypeTest()
        {
            //System.Type的实例代表了类型的元数据
            //获得Type实例的方法:
            //1. 方法GetType()
            Console.WriteLine(DateTime.Now.GetType());  //System.DateTime
            //2. 关键字typeof()
            Console.WriteLine(typeof(DateTime));    //System.DateTime
            //3. 方法Assembly.GetType()
            Console.WriteLine(Assembly.GetExecutingAssembly().GetType("SampleCode.Test.Tests"));    //SampleCode.Test.Tests
            //4. 方法Type.GetType() 需要知道对象的程序集限定名称
            Console.WriteLine(Type.GetType("System.Int32, mscorlib, Version= 2.0.0.0," + "Culture= neutral, PublicKeyToken= b77a5c561934e089")); //System.Int32

            //获取数组类型
            //可以通过对元素类型调用MakeArrayType获得该元素的数组类型
            //其中MakeArrayType可以接收一个int类型的参数创建多维数组
            Console.WriteLine(typeof(int).MakeArrayType()); //System.Int32[]
            Console.WriteLine(typeof(int).MakeArrayType(3)); //System.Int32[,,]
            //GetElementType方法可以获取数组元素的类型
            Console.WriteLine(typeof(int[]).GetElementType()); //System.Int32
            //GetArrayRank方法会返回数组的维度
            Console.WriteLine(typeof(int[,,]).GetArrayRank()); //3

            //获取嵌套类型
            foreach (var nestedType in typeof(Environment).GetNestedTypes())
            {
                Console.WriteLine(nestedType.FullName);
            }//System.Environment+SpecialFolder System.Environment + SpecialFolderOption

            //类型名称
            //在大多数情况下, FullName是Namespace和Name的结合, 除了嵌套类型和封闭的泛型类型
            //1. 嵌套类型名称: 会用+将类型与嵌套的命名空间分隔开
            Console.WriteLine(typeof(Environment.SpecialFolder).FullName);  //System.Environment+SpecialFolder
            //2. 泛型类型的名称: 泛型类型名称会带有'后缀, 如果泛型是封闭的, 则FullName还会包含额外的附加信息
            Console.WriteLine(typeof(Dictionary<int, int>));  //System.Collections.Generic.Dictionary`2[System.Int32,System.Int32]
            Console.WriteLine(typeof(Dictionary<,>));  //System.Collections.Generic.Dictionary`2[TKey,TValue]
            //3. 数组类型名称
            Console.WriteLine(typeof(int).MakeArrayType(3)); //System.Int32[,,]
        }

        #endregion

        #region 反射-实例化类型(调用构造函数)

        /// <summary>
        /// 反射-实例化类型(调用构造函数)
        /// </summary>
        [Test]
        public void InstantiationTest()
        {
            //实例化类型的方法
            //1.调用静态的Activator.CreateInstance()方法
            //Activator.CreateInstance方法接受type类型的参数, 并且可以接受可选参数
            var i = (DateTime)Activator.CreateInstance(typeof(DateTime), 2000, 01, 01);
            //2.调用ConstructorInfo.Invoke方法, 并使用Type的GetConstructor方法的返回值作为参数
            //ConstructorInfo.Invoke可以指定构造函数
            var constructorInfo = typeof(StringBuilder).GetConstructor(new[] { typeof(string) });
            var foo = constructorInfo?.Invoke(new object[] { null });

            //实例化一个类型的数组
            var ints = (int[])Activator.CreateInstance(typeof(int).MakeArrayType(), 1);

            //实例化泛型类型
            //在编译时只能实例化封闭的泛型类型, 无法实例化未绑定的泛型类型
            var list1 = (List<int>)Activator.CreateInstance(typeof(List<int>)); //OK
            var list2 = Activator.CreateInstance(typeof(List<>));       //Runtime Error
            //可以通过方法MakeGenericType方法接受参数, 将未绑定的泛型转换为封闭的泛型
            var list3 = Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(int)));

        }

        #endregion

        #region 反射-获取成员类型

        /// <summary>
        /// 反射-获取成员类型
        /// </summary>
        [Test]
        public void GetMemberInfoTest()
        {
            #region 获取类型的成员

            Console.WriteLine($"---------------获取类型的成员-------------------");

            //GetMembers()方法可以返回类型的成员, 其中会包含基类的成员
            var members = typeof(Walnut).GetMembers();
            foreach (var memberInfo in members)
            {
                Console.WriteLine(memberInfo);
            }
            Console.WriteLine();

            //GetMember()方法可以通过名称检索特定的成员, 由于有可能重载, 因此返回的是一个数组
            Console.WriteLine(typeof(Walnut).GetMember("Crack")[0]);

            //调用GetMembers方法时, 可以传递一个MemberTypes实例来限定返回的实例类型,也可以通过GetXXXs(XXX为MemberTypes的枚举)获取单种成员

            //泛型类型成员
            //可以从未绑定的泛型类型中获得成员的元数据, 也可以从封闭的泛型中获得元数据
            var unbound = typeof(IEnumerator<>).GetProperty("Current");
            var closed = typeof(IEnumerator<int>).GetProperty("Current");
            Console.WriteLine(unbound); //T Current
            Console.WriteLine(closed);  //Int32 Current
            //为绑定泛型类型的成员无法动态调用

            #endregion

            #region MemberInfo

            Console.WriteLine($"---------------MemberInfo-------------------");

            //1.MemberTypes: Custom-自定义成员类型 Filed-字段 NestedType-嵌套类型 TypeInfo-类型
            //               Constructor-构造函数 Event-事件 Method-方法 Property-属性

            //2.检索成员的元数据
            // C#结构     推荐方法            可使用名称       结果      
            // 方法       GetMethod           方法名称         MethodInfo
            // 属性       GetProperty         属性名称         PropertyInfo
            // 索引器     GetDefaultMembers                    MemberInfo[](若在C#进行编译则会包含PropertyInfo对象)
            // 字段       GetField            字段名称         FieldInfo
            // 枚举成员   GetField            成员名称         FieldInfo
            // 事件       GetEvent            事件名称         EventInfo
            // 构造器     GetConstructor                       ConstructorInfo
            // 终结器     GetMethod           "Finalize"       MethodInfo
            // 运算符     GetMethod           "op_"+运算符名称 MethodInfo
            // 嵌套类型   GetNestedType       类型名称         Type

            //3.DeclaringType:返回成员的定义类型(基类型); ReflectedType返回调用GetMembers的具体类型(子类型)
            var reflectTestMethod = typeof(ReflectTest).GetMethod("ToString");
            var objectMethod = typeof(object).GetMethod("ToString");
            Console.WriteLine($"reflectTestMethod.DeclaringType:{reflectTestMethod?.DeclaringType}");   //System.Object
            Console.WriteLine($"objectMethod.DeclaringType:{objectMethod?.DeclaringType}");             //System.Object
            Console.WriteLine($"reflectTestMethod.ReflectedType:{reflectTestMethod?.ReflectedType}");   //SampleCode.Test.Reflect.ReflectTest
            Console.WriteLine($"objectMethod.ReflectedType:{objectMethod?.ReflectedType}");        //System.Object

            //4.在MethodInfo中
            //  MethodHandle:对于用程序域中的每一个方法都是唯一的
            //  MetadataToken:对于一个程序集模块中的所有类型和成员是唯一的
            //类ReflectTest并没有重写父类的ToString方法, 因此reflectTestMethod和objectMethod的Methodhandle和MetadataToken是相同的
            Console.WriteLine($"reflectTestMethod.MethodHandle:{reflectTestMethod?.MethodHandle.Value}");   //8789057997448
            Console.WriteLine($"objectMethod.MethodHandle:{objectMethod?.MethodHandle.Value}");   //8789057997448
            Console.WriteLine($"reflectTestMethod.MethodHandle:{reflectTestMethod?.MetadataToken}");   //100664460
            Console.WriteLine($"objectMethod.MethodHandle:{objectMethod?.MetadataToken}");   //100664460

            //5.所有的*Info实例都会在第一次使用时由反射API缓存
            var method = typeof(Walnut).GetMethod("Crack");
            var member = typeof(Walnut).GetMember("Crack")[0];
            Console.WriteLine(member == method); //true

            //6.C#结构中属性、索引器、事件会自动生成一个或两个后端方法(后端方法在IL编译后会变为普通方法)
            //  C#结构        成员类型        IL中的方法
            //  属性          Property        get_XXX和set_XXX
            //  索引器        Property        get_Item和set_Item
            //  事件          Event           add_XXX和remove_XXX
            //GetMethods时会返回经IL编译后的普通方法
            foreach (var memberInfo in typeof(Test).GetMethods())
            {
                Console.Write(memberInfo.Name + " ");
            }
            Console.WriteLine();
            //每一个后端方法都会有与其关联的MethodInfo
            var pi = typeof(Test).GetProperty("X");
            var getter = pi?.GetGetMethod();
            var setter = pi?.GetSetMethod();
            //可以通过MethodInfo中的IsSpecialName属性来判断方法是否是属性、索引器或者事件的访问器
            Console.WriteLine(typeof(Test).GetMethod("get_X")?.IsSpecialName);  //true
            Console.WriteLine(typeof(Test).GetMethod("Crack")?.IsSpecialName);  //false

            #endregion
        }

        public class Walnut
        {
            private bool cracked;
            public void Crack() { }

            public override string ToString()
            {
                return cracked.ToString();
            }
        }

        public class Test
        {
            public int X { get; set; }

            public void Crack() { }
        }

        delegate string StringToString(string s);

        #endregion

        #region 反射-动态调用成员

        /// <summary>
        /// 反射-动态调用成员
        /// </summary>
        [Test]
        public void InvokeMemberInfoTest()
        {
            //当得到MethodInfo、PropertyInfo或者FieldInfo对象后, 就可以对其进行动态调用

            #region 动态调用Field和Property

            //GetValue和SetValue可以获得/设置PropertyInfo或者FieldInfo的值
            //其中第一个参数是类型的实例, 若调用静态对象则为null
            //若访问索引器需提供第二个参数作为索引器的值
            var str = "Hello";
            var pi = typeof(string).GetProperty("Length");
            var length = pi?.GetValue(str, null);
            Console.WriteLine(length);

            #endregion

            #region 动态调用Method

            //调用MethodInfo的Invoke方法, 并提供一个参数组作为方法的参数, 就可以动态的调用方法
            //若传递的参数类型错误, 则运行时会抛出异常
            //调用Getmethod时最好显式指定参数, 以避免重载方法的二义性
            Type[] parameterTypes = { typeof(int) };    //方法的参数类型
            var method = typeof(string).GetMethod("Substring", parameterTypes);
            var returnObject = method?.Invoke("stamp", new object[] { 2 });
            Console.WriteLine(returnObject);    //amp
            //可以通过MethodBase的GetParameters方法获取方法的参数
            var paramList = method?.GetParameters();
            foreach (var parameterInfo in paramList)
            {
                Console.Write(parameterInfo.ParameterType);
            }
            Console.WriteLine();

            #endregion

            #region ref和out参数

            //若需要传递ref或out参数, 可以在获得方法之前调用相应的类型的MakeByRefType
            var args = new object[] { "23", 0 };
            Type[] argTypes = { typeof(string), typeof(int).MakeByRefType() };
            var tryParse = typeof(int).GetMethod("TryParse", argTypes);
            var successfulParse = (bool)tryParse?.Invoke(null, args);
            Console.WriteLine($"{successfulParse} : {args[1]}");

            #endregion

            #region 调用泛型方法

            //GetMethod方法调用时可以显式指定参数以避免重载方法的二义性,
            //但是这种方法无法指定泛型方法参数类型,原因在于泛型参数无法指定
            Type[] whereTypes = { typeof(IEnumerable<>), typeof(Func<,>) };
            var where = typeof(Enumerable).GetMethod("Where", whereTypes);
            //可以用以下方法获取指定重载
            var unboundMethod = (from m in typeof(Enumerable).GetMethods()
                                 where m.Name == "Where" && m.IsGenericMethod
                                 let parameters = m.GetParameters()
                                 where parameters.Length == 2
                                 let genArg = m.GetGenericArguments().First()
                                 let enumerableOfT = typeof(IEnumerable<>).MakeGenericType(genArg)
                                 let funcOfTBool = typeof(Func<,>).MakeGenericType(genArg, typeof(bool))
                                 where parameters[0].ParameterType == enumerableOfT && parameters[1].ParameterType == funcOfTBool
                                 select m).FirstOrDefault();
            //通过MakeGenericMethod方法封闭参数
            var closedMethod = unboundMethod?.MakeGenericMethod(typeof(int));
            //调用泛型方法
            int[] sourse = { 3, 4, 5, 6, 7, 8 };
            Func<int, bool> predicate = x => x % 2 == 1;
            var results = (IEnumerable<int>)closedMethod?.Invoke(null, new object[] { sourse, predicate });
            foreach (var result in results)
            {
                Console.Write(result + " ");
            }
            Console.WriteLine();

            #endregion

            #region 通过委托提高动态调用性能

            //如果需要在一个循环里重复调用某个方法, 则可为目标动态方法动态实例化一个委托,
            //这样可以将微秒级的开销降低到纳秒级
            var mi = typeof(string).GetMethod("Tirm", new Type[0]);
            var tirm = (StringToString)Delegate.CreateDelegate(typeof(StringToString), mi);
            for (int i = 0; i < 100000; i++)
            {
                tirm("test");
            }

            #endregion
        }

        #endregion

        #region 反射-访问非公有类型

        /// <summary>
        /// 反射-访问非公有类型
        /// </summary>
        [Test]
        public void ReflectNoPublicTest()
        {
            //类型上所有检测元数据的方法都含有BindingFlags枚举的重载
            //BindingFlags枚举可以更改默认筛选标准, 默认为 BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public
            //BindingFlags常用枚举值: 1.NonPublic-指定非公共成员要包括在搜索中(internal、protected、protected internal以及private)
            //                        2.Public-指定公共成员要包括在搜索中
            //                        3.Static-指定静态成员要包括在搜索中
            //                        4.Instance-指定实例成员要包括在搜索中
            //                        5.DeclaredOnly-指定只应考虑在所提供类型的层次结构级别上声明的成员,不考虑继承的成员
            //获取Walnut的字段cracked
            var walnut = new Walnut();
            var fi = typeof(Walnut).GetField("cracked", BindingFlags.NonPublic | BindingFlags.Instance);
            Console.WriteLine(fi?.GetValue(walnut));    //false

        }

        #endregion

        #region 反射-检索特性

        /// <summary>
        /// 反射-检索特性
        /// </summary>
        [Test(Description = "测试")]
        public void ReflectAttributeTest()
        {
            //可以使用Attribute.GetCustomAttribute或Attribute.GetCustomAttributes方法检索特性
            //1.获取指定方法的指定特性
            var mi = typeof(ReflectTest).GetMethod("ReflectAttributeTest");
            if (mi != null)
            {
                var att = (TestAttribute)Attribute.GetCustomAttribute(mi, typeof(TestAttribute));
                Console.WriteLine($"方法名：{mi.Name} 特性内容：{att?.Description}");
            }
            //2.获取指定方法的所有特性
            var atts = Attribute.GetCustomAttributes(mi);
            foreach (var att in atts)
            {
                Console.WriteLine(att);
            }
        }

        #endregion

        #region 调用未知类型的泛型接口成员

        /// <summary>
        /// 调用未知类型的泛型接口成员
        /// </summary>
        [Test]
        public void ToStringExTest()
        {
            //Console.WriteLine(ToStringEx(new List<int> { 1, 2, 3, 4 }));
            //Console.WriteLine(ToStringEx("xyyzzz".GroupBy(x => x)));
            Console.WriteLine(ToStringEx(1));
        }

        public string ToStringEx(object value)
        {
            if (value == null) return "<null>";
            if (value.GetType().IsPrimitive) return value.ToString();

            var sb = new StringBuilder();

            if (value is IList list)
            {
                sb.Append("List of " + list.Count + " item: ");
            }

            var closedIGrouping = value.GetType().GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IGrouping<,>));
            if (closedIGrouping != null)
            {
                var pi = closedIGrouping.GetProperty("Key");
                object key = pi.GetValue(value, null);
                sb.Append("Group with key=" + key + ": ");
            }

            if (value is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    sb.Append(ToStringEx(item) + " ");
                }
            }

            if (sb.Length == 0)
            {
                sb.Append(value);
            }

            return "\n\t" + sb;
        }

        #endregion
    }
}