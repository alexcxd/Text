namespace DotNetTest.Reflect
{
    public class User
    {
        //public string Username { get; set; }

        /// <summary>
        /// 私有字段
        /// </summary>
        private string _Username;

        /// <summary>
        /// 公有属性
        /// </summary>
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        /// <summary>
        /// 私有属性
        /// </summary>
        private string Password { get; set; }

        /// <summary>
        /// 公有字段
        /// </summary>
        public string PublicS;

        private string PrivateS;


    }
}