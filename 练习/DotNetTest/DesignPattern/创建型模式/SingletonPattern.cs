using System;
using System.Runtime.InteropServices;

namespace DotNetTest.DesignPattern
{
    public class SingletonPattern
    {
        public static void SingletonPatternMain()
        {
            var instanse = Singleton.GetSingleton();
            var instanse1 = Singleton.GetSingleton();

            Console.WriteLine(instanse.Equals(instanse1));

        }
        /// <summary>
        /// 获取引用类型的内存地址方法    
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string GetMemory(object o)
        {
            GCHandle h = GCHandle.Alloc(o, GCHandleType.WeakTrackResurrection);

            IntPtr addr = GCHandle.ToIntPtr(h);

            return "0x" + addr.ToString("X");
        }
    }

    #region 懒汉式

    /// <summary>
    /// 线程不安全的
    /// </summary>
    public class Singleton
    {
        private static Singleton instanse;

        public static Singleton GetSingleton()
        {
            if (instanse == null)
            {
                instanse = new Singleton();
            }
            return instanse;
        }
    }

    /// <summary>
    /// 线程安全的
    /// </summary>
    public class SingletonSafe
    {
        private static Singleton instanse;
        private static readonly object objlock = new object();

        public static Singleton GetSingleton()
        {
            lock (objlock)
            {
                if (instanse == null)
                {
                    instanse = new Singleton();
                }
            }
            return instanse;
        }
    }

    /// <summary>
    /// 线程安全的 双重锁定的
    /// </summary>
    /// 不需要每一次都去锁定线程
    public class SingletonSafe2
    {
        private static Singleton instanse;
        private static readonly object objlock = new object();

        public static Singleton GetSingleton()
        {
            if (instanse == null)
            {
                lock (objlock)
                {
                    instanse = new Singleton();

                }
            }
            return instanse;
        }
    }
    #endregion

    #region 饿汉式
    /// <summary>
    /// 静态初始化
    /// </summary>
    /// 线程安全的
    public sealed class StateSingleton
    {
        private static readonly Singleton instanse = new Singleton();

        public static Singleton GetSingleton()
        {
            return instanse;
        }
    }

    #endregion
}