using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace DotNetTest.AssemblyTest
{
    class DynamicCreateAssembly
    {
        //动态加载程序集
        public static void DynamicCreateAssemblyMain()
        {

            var body=File.ReadAllText("DynamicCreateAssemblyText.txt").Replace("\n","").Replace("\r","").Replace("\t", "");
            var first = "using System;class Driver{public static void Run(){";
            var end = "}}";
            var sb = new StringBuilder();
            sb.Append($"{first} {body} {end}");
            /*while (true)
            {
                var str = Console.ReadLine();
                if (str.Equals("0000"))
                {
                    break;
                }
                sb.Append(str);
            }*/

            string returnData = null;
            CompilerResults compilerResults = null;

            using (var provider = new CSharpCodeProvider())
            {
                var options = new CompilerParameters();
                options.GenerateInMemory = true;

                //编译代码并创建程序集
                var result = provider.CompileAssemblyFromSource(options, sb.ToString());

                if (result.Errors.HasErrors)
                {
                    //输出错误信息
                    var errorMessage = new StringBuilder();
                    foreach (CompilerError error in result.Errors)
                    {
                        errorMessage.AppendFormat("{0}: {1}\n", error.Line, error.ErrorText);
                    }
                    returnData = errorMessage.ToString();
                }
                else
                {
                    //切换控制台
                    TextWriter temp = Console.Out;
                    var write = new StringWriter();
                    Console.SetOut(write);

                    //通过反射拿到所需类的Type，再通过InvokeMember运行其中的方法
                    Type driverType = result.CompiledAssembly.GetType("Driver");
                    var instance = Activator.CreateInstance(driverType);
                    driverType.InvokeMember("Run", BindingFlags.InvokeMethod |
                                                   BindingFlags.Static | 
                                                   BindingFlags.Public, 
                                                   null, null, null);
                    Console.SetOut(temp);
                    returnData = write.ToString();
                }
                Console.WriteLine(returnData);
            }

            


        }

    }
}
