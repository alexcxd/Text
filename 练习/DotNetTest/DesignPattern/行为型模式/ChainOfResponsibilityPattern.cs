using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTest.DesignPattern
{
    /// <summary>
    /// 责任链模式
    /// </summary>
    /// 使多个对象都有机会处理请求，从而避免请求发送者和接收者间的耦合。
    ///将这个对象连成一个链，并沿着这条链传递请求，知道可以处理他
    class ChainOfResponsibilityPattern
    {
        public static void ChainOfResponsibilityPatternMain()
        {
            ConcreteHandlerA handlerA = new ConcreteHandlerA();
            ConcreteHandlerB handlerB = new ConcreteHandlerB();

            handlerA.SetSuccessor(handlerB);

            handlerA.HandleRequest(1);
            handlerA.HandleRequest(11);


        }
    }

    public abstract class Handler
    {
        protected Handler successor;

        public void SetSuccessor(Handler successor)
        {
            this.successor = successor;
        }

        public abstract void HandleRequest(int request);
    }


    public class ConcreteHandlerA : Handler
    {
        public override void HandleRequest(int request)
        {
            if(request < 10)
            {
                Console.WriteLine("request 小于10");
                return;
            }

            if (successor != null)
            {
                successor.HandleRequest(request);
            }
        }
    }


    public class ConcreteHandlerB : Handler
    {
        public override void HandleRequest(int request)
        {
            if (request > 10)
            {
                Console.WriteLine("request 大于10");
                return;
            }

            if (successor != null)
            {
                successor.HandleRequest(request);
            }
        }
    }
}
