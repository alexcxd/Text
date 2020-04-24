using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotNetTest.AdoTest;
using Microsoft.Win32.SafeHandles;
using Test.DesignPattern;
using DotNetTest.AssemblyTest;
using DotNetTest.AttributeTest;
using DotNetTest.BugTest;
using DotNetTest.ClassTest;
using DotNetTest.DataStructrue;
using DotNetTest.FakerNet;
using DotNetTest.FileReadAndWrite;
using DotNetTest.Gather;
using DotNetTest.Json;
using DotNetTest.Random;
using DotNetTest.Reflect;
using DotNetTest.Test;
using DotNetTest.ThreadTest;
using DotNetTest.正则表达式;
using DotNetTest.访问级别;
using Newtonsoft.Json;
using Test.Gather;

namespace DotNetTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //正则测试
            /*EasyRegex easyRegex = new EasyRegex();
            easyRegex.demo1();*/

            //数据结构测试
            /*DataStructrueMain dataStructrue = new DataStructrueMain();
            dataStructrue.WriteLine();*/

            //C# 访问级别测试 public private protected internal(内部的)
            /*AccessLevelMain accessLevelMain = new AccessLevelMain();
            accessLevelMain.WriteLine();*/

            //反射测试
            /*ReflectMain reflectMain = new ReflectMain();
            reflectMain.WriteLine();*/

            //Json测试
            /*JsonMian.Write();*/

            //程序集测试
            //AssemblyMain.Write();

            //获取注释和属性的键值关系对
            /*Info info = new Info();
            info.LoadFile();*/

            //对象属性映射(效率太低，除了用着方便没啥用)
            /*ObjectToObject test = new ObjectToObject();
            test.Test();*/

            //String类相关测试,未完成
            /*ClassTest.Main stringTest = new ClassTest.Main();
            stringTest.WriteLine();*/

            //bug再现测试
            /*BugTest.Main bugMain = new BugTest.Main();
            bugMain.WriteLine();*/

            /*//特性测试
            AttributeTestMain.Write();*/

            //随机数测试
            //RandomMain.Write();

            /*//Linq测试
            LinqMain linqMain = new LinqMain();
            linqMain.Write();*/

            /*//指针测试
            PointerMain pointerMain = new PointerMain();
            pointerMain.Write();*/

            //集合测试
            /*GatherMain gatherMain = new GatherMain();
            gatherMain.Write();*/

            //设计模式
            //DesignPatternMain.Write();

            //线程测试
            TreadMain.Write();

            //数据生成
            //FakerMain.Write();

            //Ado实验
            //AdoMainTest.Write();

            Console.Read();
        }
    }
}
