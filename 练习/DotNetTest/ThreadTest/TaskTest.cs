using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetTest.ThreadTest
{
    /// <summary>
    /// 任务
    /// 包含的类抽象出了线程功能，在后台使用TreadPool
    /// </summary>
    public class TaskTest
    {
        public static void TaskTestMain()
        {
            TaskTest1();
        }

        /// <summary>
        /// 启动和创建任务
        /// </summary>
        private static void TaskTest1()
        {
            Console.WriteLine("主线程启动。");

            //方法一：传入Action<object>
            new Task(StartCode, 1).Start();

            //方法二：使用lamada表达式
            var task1 = new Task<int>(x => Sum((int)x), 1000000000);
            task1.Start();
            task1.Wait();
            Console.WriteLine($"task1结果：{task1.Result}");

            //方法三：使用TaskFactory
            var tf = new TaskFactory();
            tf.StartNew(StartCode, 2);

            //方法四：使用Task.Factory
            var task2 = Task.Factory.StartNew(StartCode, 3);

            Console.WriteLine("主线程结束。");
            Thread.Sleep(1000);
        }

        private static void StartCode(object i)
        {
            Console.WriteLine($"开始执行任务-{i}");
            Thread.Sleep(1000);
        }

        private static int Sum(int i)
        {
            var sum = 0;
            for (; i > 0; i--)
            {
                //用于打开整数溢出检查
                //与之相反的是unchecked，用于关闭整数溢出检查
                //默认是关闭状态
                checked
                {
                    sum += i;
                }
            }
            return sum;
        }
    }
}