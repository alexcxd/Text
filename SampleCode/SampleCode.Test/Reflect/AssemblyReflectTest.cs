using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SampleCode.Reflect;

namespace SampleCode.Test.Reflect
{
    /// <summary>
    /// 程序集反射
    /// </summary>
    [TestFixture]
    public class AssemblyReflectTest
    {
        [Test]
        public void AssemblyReflectCodeTest()
        {
            //反射外部程序集
            var assemblyEntity = Assembly.Load("SampleCode");

            //反射当前程序集
            var assemblyThis = this.GetType().Assembly;

            //通过某一父类查找该父类的所有子类
            var assemblyEntityTypesInfo = assemblyEntity.DefinedTypes.Where(m => m.BaseType == typeof(EntityBase)).ToList();

            var assemblyEntityTypes = assemblyEntity.GetTypes().Where(m => m.BaseType == typeof(EntityBase));

            //获取当前代码正在执行的程序集
            var assemblyNow = Assembly.GetExecutingAssembly();
        }
    }
}