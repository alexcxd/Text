﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace Test.DesignPattern
{
    /// <summary>
    /// 代理模式
    /// </summary>
    /// 权限控制，添加功能等
    class ProxyPattern
    {
        public static void ProxyPatternMain()
        {
            //静态代理模式
            Proxy proxy = new Proxy(new RealSubject());
            proxy.Request();

            //动态代理 继承RealProxy(.net自带)



            
        }
    }

    public abstract class Subject
    {
        public abstract void Request();
    }

    public class RealSubject : Subject
    {
        public override void Request()
        {
            Console.WriteLine("真实请求");
        }
    }

    public class Proxy : Subject
    {
        private RealSubject realSubject;

        public Proxy(RealSubject realSubject)
        {
            this.realSubject = realSubject;
        }

        public override void Request()
        {
            if(realSubject == null)
            {
                realSubject = new RealSubject();
            }
            Console.WriteLine("代理类做了一系列请求");
            realSubject.Request();
        }
    }

}
