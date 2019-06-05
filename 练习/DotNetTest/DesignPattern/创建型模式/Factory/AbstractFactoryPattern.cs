using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DesignPattern.Factory
{
    /// <summary>
    /// 抽象工厂模式
    /// </summary>
    /// 围绕一个超级工厂创建其他工厂，该超级工厂又称为其他工厂的工厂
    class AbstractFactoryPattern
    {
        public static void AbstractFactoryPatternMain()
        {
            //操作MySql数据库
            var mysqlFactory = new MySqlFactory();
            var mysqlDapartment = mysqlFactory.CreateDapartment();
            mysqlDapartment.Insert();
            mysqlDapartment.Delete();
            var mysqlUser = mysqlFactory.CreateUser();
            mysqlUser.Insert();
            mysqlUser.Delete();

            //操作Oracle数据库
            var oracleFactory = new OracleFactory();
            var oracleDapartment = mysqlFactory.CreateDapartment();
            mysqlDapartment.Insert();
            mysqlDapartment.Delete();
            var oracleUser = mysqlFactory.CreateUser();
            mysqlUser.Insert();
            mysqlUser.Delete();
        }
    }

    public interface IDao
    {
        void Insert();
        void Delete();
    }

    public class MySQlUserDao : IDao
    {
        public void Delete()
        {
            Console.WriteLine("在MySql数据库User表删除了一条数据");
        }

        public void Insert()
        {
            Console.WriteLine("在MySql数据库User表添加了一条数据");
        }
    }

    public class OracleUserDao : IDao
    {
        public void Delete()
        {
            Console.WriteLine("在Oracle数据库User表删除了一条数据");
        }

        public void Insert()
        {
            Console.WriteLine("在Oracle数据库User表添加了一条数据");
        }
    }

    public class MySQlDapartmentDao : IDao
    {
        public void Delete()
        {
            Console.WriteLine("在MySql数据库Dapartment表删除了一条数据");
        }

        public void Insert()
        {
            Console.WriteLine("在MySql数据库Dapartment表添加了一条数据");
        }
    }

    public class OracleDapartmentDao : IDao
    {
        public void Delete()
        {
            Console.WriteLine("在Oracle数据库Dapartment表删除了一条数据");
        }

        public void Insert()
        {
            Console.WriteLine("在Oracle数据库Dapartment表添加了一条数据");
        }
    }

    public interface IFactory
    {
        IDao CreateUser();
        IDao CreateDapartment();
    }

    public class MySqlFactory : IFactory
    {
        public IDao CreateDapartment()
        {
            return new MySQlDapartmentDao();
        }

        public IDao CreateUser()
        {
            return new MySQlUserDao();
        }
    }

    public class OracleFactory : IFactory
    {
        public IDao CreateDapartment()
        {
            return new OracleDapartmentDao();
        }

        public IDao CreateUser()
        {
            return new OracleUserDao();
        }
    }
}
