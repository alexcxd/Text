using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SampleCode.Test.MultiThread
{
    /// <summary>
    /// 异步函数
    /// 主要涉及await和async
    /// </summary>
    [TestFixture]
    public class AsynchronousFunctionTest
    {
        #region 等待

        /// <summary>
        /// 等待
        /// </summary>
        [Test]
        public async Task WaitTest()
        {
            //async修饰符是为了指示编译器将await作为一个关键字而非标识符避免二义性
            //async只支持返回类型为void、Task或者Task<T>的方法(Lambda表达式)
            //添加了async的函数被称为异步函数

            //await关键字可以简便的附加任务的延续
            var result1 = await Task.Run(() => "task");
            //等价于
            var awaiter = Task.Run(() => "task").GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                var result2 = awaiter.GetResult();
            });

            //通常情况下await等待的是一个任务, 但实际上只要对象拥有GetAwaiter方法,
            //且返回值是一个可等待的对象, 则都可以等待

            //await最大的优势在于可以出现除lock表达式、unsafe上下文或Main方法中的任意位置, 即可以方便的获得本地状态
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(await Task.Run(() => i));
            }
            //在不使用await的实现(递归)
            //ConsoleI(0);
            //Thread.Sleep(500);
        }

        public void ConsoleI(int i)
        {
            var awaiter1 = Task.Run(() => i).GetAwaiter();
            awaiter1.OnCompleted(() =>
            {
                Console.WriteLine(awaiter1.GetResult());
                if (++i < 10)
                {
                    ConsoleI(i);
                }
            });
        }

        #endregion

        #region 编写异步函数

        /// <summary>
        /// 编写异步函数
        /// </summary>
        [Test]
        public async Task AsynchronousFunctionCodeTest()
        {
            //异步函数程序设计基本原则
            //1.首先以同步的方式实现方法
            //2.其次, 将同步方法调用改为异步方法调用, 并使用await
            //3.除了最顶级的方法之外, 将异步方法的返回类型改为Task或者Task<TResult>, 使其变为可等待的方法

            //编写异步函数时, 要讲返回类型由void改为Task, 这样方法本身就是可以异步调用的了
            //方法体内不需要显示返回任务, 编译器会负责生成任务(TaskCompletionSource),
            //并在方法完成之前或出现异常时触发任务
            await PrintAnswerToLift1();
            //等价于
            await PrintAnswerToLift2();

            //执行异步调用图
            //执行流程:
            //1. Go调用PrintAnswerToLift, PrintAnswerToLift调用GetAnswerToLift, GetAnswerToLift调用Delay
            //2. 调用Delay后调用await, await会将执行点返回到PrintAnswerToLift的await上, 继而返回到Go
            //3. 5秒后Delay的延续会被触发, 执行点返回到GetAnswerToLift并在线程池线程上执行
            //4. 随后GetAnswerToLift的其他语句将执行, Task<int>得到结构42并结束
            //5. 之后执行PrintAnswerToLift的延续(剩余代码)
            //6. 最后执行Go的延续
            await Go3();

            //并行性
            //调用异步方法不使用await等待就可以令异步方法和后续代码并行执行
        }

        public async Task PrintAnswerToLift1()
        {
            await Task.Delay(1000);
            var answer = 21 * 2;
            Console.WriteLine(answer);
        }

        public Task PrintAnswerToLift2()
        {
            var tcs = new TaskCompletionSource<object>();
            var awaiter = Task.Delay(1000).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                try
                {
                    awaiter.GetResult();
                    var answer = 21 * 2;
                    Console.WriteLine(answer);
                    tcs.SetResult(null);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });

            return tcs.Task;
        }

        public async Task Go3()
        {
            var task = PrintAnswerToLift3();
            await task;
            Console.WriteLine("Done");
        }

        public async Task PrintAnswerToLift3()
        {
            var task = GetAnswerToLift3();
            var answer = await task;
            Console.WriteLine(answer);
        }

        public async Task<int> GetAnswerToLift3()
        {
            await Task.Delay(1000);
            var answer = 21 * 2;
            return answer;
        }

        #endregion

        #region 异步Lambda表达式

        /// <summary>
        /// 异步Lambda表达式
        /// </summary>
        [Test]
        public void AsynLambdaTest()
        {
            //匿名方法可以通过添加async关键字变为异步方法
            Func<Task> unnamed = async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine("AsynLambda");
            };
        }

        #endregion
    }
}