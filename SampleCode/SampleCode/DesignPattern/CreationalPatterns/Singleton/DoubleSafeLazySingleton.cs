namespace SampleCode.DesignPattern.CreationalPatterns.Singleton
{
    /// <summary>
    /// 懒汉式单例 - 双重锁定
    /// 不需要每一次都去锁定线程
    /// </summary>
    public class DoubleSafeLazySingleton
    {
        /// <summary>
        ///  1、声明一个静态实例，不给它初始化。等什么时候用就什么时候初始化。节省资源
        /// </summary>
        private static DoubleSafeLazySingleton instance;

        /// <summary>
        /// 2、声明锁对象
        /// </summary>
        private static readonly object objlock = new object();

        /// <summary>
        /// 3、依然私有化构造器，对外不让new
        /// </summary>
        private DoubleSafeLazySingleton() { }

        /// <summary>
        /// 4、对外提供一个获取实例的方法，因为静态属性没有实例化。
        /// 假如高并发的时候，有可能会同时调用该方法。造成new出多个实例。
        /// 所以需要加线程锁，因此调用效率不高在方法上加同步，是整个方法都同步。效率不高
        /// </summary>
        public static DoubleSafeLazySingleton GetInstance()
        {
            // 不需要每一次都去锁定线程
            if (instance == null)
            {
                // 线程锁
                lock (objlock)
                {
                    if (instance == null)
                    {
                        instance = new DoubleSafeLazySingleton();
                    }
                }
            }

            //之后第二次再调用的时候就已经初始化了，不用再new。直接返回
            return instance;
        }
    }
}