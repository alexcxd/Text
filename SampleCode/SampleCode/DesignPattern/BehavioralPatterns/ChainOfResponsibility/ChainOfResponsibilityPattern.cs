using System;

namespace SampleCode.DesignPattern.BehavioralPatterns.ChainOfResponsibility
{
    /// <summary>
    /// 责任链模式
    /// </summary>
    /// 使多个对象都有机会处理请求，从而避免请求发送者和接收者间的耦合。
    ///将这个对象连成一个链，并沿着这条链传递请求，知道可以处理他
    public class ChainOfResponsibilityPattern
    {
        public static void ChainOfResponsibilityPatternMain()
        {
            var handlerA = new ConcreteHandlerA();
            var handlerB = new ConcreteHandlerB();

            handlerA.SetSuccessor(handlerB);

            handlerA.HandleRequest(1);
            handlerA.HandleRequest(11);


        }
    }

    public abstract class Handler
    {
        protected Handler Successor;

        public void SetSuccessor(Handler successor)
        {
            this.Successor = successor;
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

            Successor?.HandleRequest(request);
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

            if (Successor != null)
            {
                Successor.HandleRequest(request);
            }
        }
    }
}
