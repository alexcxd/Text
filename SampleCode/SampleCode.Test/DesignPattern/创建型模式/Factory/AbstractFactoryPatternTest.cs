﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Test.DesignPattern.Factory;

namespace SampleCode.Test.DesignPattern.Factory
{
    public class AbstractFactoryPatternTest
    {
        /// <summary>
        /// 抽象工厂模式
        /// </summary>
        [Test]
        public void AbstractFactoryPatternCodeTest()
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
}
