namespace SampleCode.DesignPattern.CreationalPatterns.Singleton
{
    /// <summary>
    /// 静态内部类方式
    /// </summary>
    public class InternalStaticSingleton
    {
        /// <summary>
        /// 1、私有化构造器
        /// </summary>
        private InternalStaticSingleton() { }
        
        /// <summary>
        /// 2、声明一个静态内部类,在静态内部类内部提供一个外部类的实例（常量，不可改变）
        /// 初始化 SingletonClassInstance 的时候不会初始化SingletonClassInstance，实现了延时加载。并且线程安全
        /// </summary>
        private static class SingletonClassInstance
        {
            //该实例只读，不管谁都不能修改
            public static readonly InternalStaticSingleton Instance = new InternalStaticSingleton();
        }
        
        /// <summary>
        /// 3、对外提供一个获取实例的方法：直接返回静态内部类中的那个常量实例调用的时候没有同步等待，所以效率也高
        /// </summary>
        /// <returns></returns>
        public static InternalStaticSingleton GetInstance()
        {
            return SingletonClassInstance.Instance;
        }
    }
}