using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTest.DesignPattern
{
    /// <summary>
    /// 状态模式
    /// </summary>
    /// 当一个对象的内在状态发送变化时允许改变行为
    /// 作用：将状态判断逻辑每个状态类里面，可以简化判断的逻辑。
    class StatePattern
    {
        public static void StatePatternMain()
        {
            Context context = new Context(new ConcreteStateA());
            context.Request();
            context.Request();
            context.Request();
            context.Request();
        }
    }

    public abstract class State
    {
        public abstract void Handle(Context context);
    }

    public class ConcreteStateA : State
    {
        public override void Handle(Context context)
        {
            context.State = new ConcreteStateB();
        }
    }

    public class ConcreteStateB : State
    {
        public override void Handle(Context context)
        {
            context.State = new ConcreteStateA();
        }
    }

    public class Context
    {
        public Context(State state)
        {
            this.state = state;
        }

        private State state;
        public State State
        {
            get { return state; }
            set
            {
                Console.WriteLine($"当前的状态为：{state.GetType().Name}");
                state = value;
            }
        }

        public void Request()
        {
            if(state == null)
            {
                throw new ArgumentNullException("state");
            }
            state.Handle(this);
        }
    }
}
