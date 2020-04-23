using System.Collections.Generic;

namespace SampleCode.Reflect
{
    public interface IEntityBase
    {
        bool Delstatus { get; set; }
    }

    public class EntityBase : IEntityBase
    {
        public int Id { get; set; }

        public bool Delstatus { get; set; }
    }

    public class Mall : EntityBase
    {
        public string MallName { get; set; }
    }


    public class User
    {
        public User() { }

        public User(string password)
        {
            this.Password = password;
        }
        public User(List<int> ids)
        {

        }

        //public string Username { get; set; }

        /// <summary>
        /// 私有字段
        /// </summary>
        private string username;

        /// <summary>
        /// 公有属性
        /// </summary>
        public string Username
        {
            get => username;
            set => username = value;
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