using System;

namespace SampleCode.DesignPattern.BehavioralPatterns
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
            var context = new StateContext(new ConcreteState1());
            context.Handle1();
            context.Handle2();
        }
    }

    /// <summary>
    /// 抽象状态
    /// </summary>
    public abstract class State
    {
        public abstract void Handle1(StateContext context);

        public abstract void Handle2(StateContext context);
    }

    /// <summary>
    /// 具体状态角色
    /// </summary>
    public class ConcreteState1 : State
    {
        public override void Handle1(StateContext context)
        {
            // 本状态下必须处理的逻辑
        }
        public override void Handle2(StateContext context)
        {
            //设置当前状态为stat2
            context.State = StateContext.State2;
            context.State.Handle2(context);
        }
    }

    /// <summary>
    /// 具体状态角色
    /// </summary>
    public class ConcreteState2 : State
    {
        public override void Handle1(StateContext context)
        {
            //设置当前状态为stat2
            context.State = StateContext.State1;
            context.State.Handle1(context);
        }
        public override void Handle2(StateContext context)
        {
            // 本状态下必须处理的逻辑
        }
    }

    /// <summary>
    /// 环境角色
    /// </summary>
    public class StateContext
    {
        #region 构造
        public StateContext(State state)
        {
            this.state = state;
        }

        #endregion

        #region 常量

        /// <summary>
        /// 常量-状态1
        /// </summary>
        public static readonly State State1 = new ConcreteState1();

        /// <summary>
        /// 常量-状态2
        /// </summary>
        public static readonly State State2 = new ConcreteState2();

        #endregion

        private State state;
        public State State
        {
            get => state;
            set
            {
                Console.WriteLine($"当前的状态为：{state.GetType().Name}");
                state = value;
            }
        }

        /// <summary>
        /// 行为委托 - 行为1
        /// </summary>
        public void Handle1()
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }
            state.Handle1(this);
        }

        /// <summary>
        /// 行为委托 - 行为2
        /// </summary>
        public void Handle2()
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }
            state.Handle2(this);
        }
    }
}
