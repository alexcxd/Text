using System;

namespace SampleCode.DesignPattern.CreationalPatterns.Factory
{
    /// <summary>
    /// 抽象工厂模式
    /// 抽象工厂利用一个独立的接口或者抽象类提供“一组相关的对象”
    /// 围绕一个超级工厂创建其他工厂，该超级工厂又称为其他工厂的工厂
    /// </summary>
    public class AbstractFactoryPattern
    {
        public static void AbstractFactoryPatternMain()
        {
            //操作MySql数据库
            var mysqlFactory = new MySqlFactory();
            var mysqlDepartment = mysqlFactory.CreateDapartment();
            mysqlDepartment.Insert();
            mysqlDepartment.Delete();
            var mysqlUser = mysqlFactory.CreateUser();
            mysqlUser.Insert();
            mysqlUser.Delete();

            //操作Oracle数据库
            var oracleFactory = new OracleFactory();
            var oracleDepartment = oracleFactory.CreateDapartment();
            oracleDepartment.Insert();
            oracleDepartment.Delete();
            var oracleUser = mysqlFactory.CreateUser();
            oracleUser.Insert();
            oracleUser.Delete();
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

    public class MySQlDepartmentDao : IDao
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

    public class OracleDepartmentDao : IDao
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

    public partial interface IAbstractFactory
    {
        IDao CreateUser();
        IDao CreateDapartment();
    }

    public class MySqlFactory : IAbstractFactory
    {
        public IDao CreateDapartment()
        {
            return new MySQlDepartmentDao();
        }

        public IDao CreateUser()
        {
            return new MySQlUserDao();
        }
    }

    public class OracleFactory : IAbstractFactory
    {
        public IDao CreateDapartment()
        {
            return new OracleDepartmentDao();
        }

        public IDao CreateUser()
        {
            return new OracleUserDao();
        }
    }
}
