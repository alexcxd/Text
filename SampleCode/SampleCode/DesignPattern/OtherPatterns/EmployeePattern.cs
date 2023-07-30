namespace SampleCode.DesignPattern.OtherPatterns
{
    /// <summary>
    /// 雇工模式
    /// 它为一组类提供通用的功能，而不需要类实现这些功能
    /// </summary>
    public class EmployeePattern
    {

        /// <summary>
        /// 
        /// </summary>
        public void EmployeePatternMain()
        {
            var servant = new Servant();
            var serviced1 = new Serviced1();
            servant.Service(serviced1);
            var serviced2 = new Serviced2();
            servant.Service(serviced2);
        }

        #region 雇工模式

        /// <summary>
        /// 抽象功能角色IServiced
        /// </summary>
        public interface IServiced
        {
            /// <summary>
            /// 具有的特质或功能
            /// </summary>
            void Serviced();
        }

        /// <summary>
        /// 具体功能角色Service1
        /// </summary>
        public class Serviced1 : IServiced
        {
            public void Serviced()
            {
                // 具体功能
            }
        }

        /// <summary>
        /// 具体功能角色Service1
        /// </summary>
        public class Serviced2 : IServiced
        {
            public void Serviced()
            {
                // 具体功能
            }
        }

        /// <summary>
        /// 雇工角色
        /// </summary>
        public class Servant
        {
            /// <summary>
            /// 服务内容
            /// </summary>
            /// <param name="serviceFuture"></param>
            public void Service(IServiced serviceFuture)
            {
                serviceFuture.Serviced();
            }
        }

        #endregion
    }
}