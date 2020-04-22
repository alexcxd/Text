using System;

namespace SampleCode.DesignPattern.行为型模式
{
    /// <summary>
    /// 状态模式
    /// </summary>
    /// 当一个对象的内在状态发送变化时允许改变行为
    /// 作用：将状态判断逻辑每个状态类里面，可以简化判断的逻辑。
    public class StatePattern
    {
        public static void StatePatternMain()
        {
            StateContext context = new StateContext(new ConcreteStateA());
            context.Request();
            context.Request();
            context.Request();
            context.Request();
        }
    }

    public abstract class State
    {
        public abstract void Handle(StateContext context);
    }

    public class ConcreteStateA : State
    {
        public override void Handle(StateContext context)
        {
            context.State = new ConcreteStateB();
        }
    }

    public class ConcreteStateB : State
    {
        public override void Handle(StateContext context)
        {
            context.State = new ConcreteStateA();
        }
    }

    public class StateContext
    {
        public StateContext(State state)
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
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }
            state.Handle(this);
        }
    }
}
