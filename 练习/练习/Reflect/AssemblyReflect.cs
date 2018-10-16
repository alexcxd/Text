using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace 练习.Reflect
{
    class AssemblyReflect
    {
        /// <summary>
        /// 反射程序集
        /// </summary>
        public void Demo1()
        {
            //反射外部程序集
            var assemblyEntity = Assembly.Load("Entity");

            //反射当前程序集
            var assemblyThis = this.GetType().Assembly;

            //通过某一父类查找该父类的所有子类
            var assemblyEntityTypesInfo = assemblyEntity.DefinedTypes.Where(m => m.BaseType == typeof(EntityBase)).ToList();

            var assemblyEntityTypes = assemblyEntity.GetTypes().Where(m => m.BaseType == typeof(EntityBase));

            //获取当前代码正在执行的程序集
            var ass1 = Assembly.GetExecutingAssembly();

        }

    }
}
