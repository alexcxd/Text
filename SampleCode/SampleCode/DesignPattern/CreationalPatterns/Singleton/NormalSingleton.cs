namespace SampleCode.DesignPattern.CreationalPatterns.Singleton
{
    /// <summary>
    /// 饿汉式单例：所谓饿汉式，就是比较饿。当类一加载的时候就直接new了一个静态实例。不管后面有没有用到该实例
    /// </summary>
    public class NormalSingleton
    {
        /// <summary>
        /// 1、提供一个静态实例，当类加载器加载该类时就会new一个出来
        /// </summary>
        private static readonly NormalSingleton instanse = new NormalSingleton();

        /// <summary>
        /// 2、私有化构造器:外部是不能直接new该对象的
        /// </summary>
        private NormalSingleton() { }

        /// <summary>
        ///  3、对外提供一个公共方法来获取这个唯一对象
        /// </summary>
        /// <returns></returns>
        public static NormalSingleton GetSingleton()
        {
            return instanse;
        }
    }
}