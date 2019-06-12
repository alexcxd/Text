using System;
using System.Collections;
using System.Collections.Generic;
using DotNetTest.DataStructrue.LinkList;

namespace DotNetTest.Gather
{
    /// <summary>
    /// 栈
    /// </summary>
    public class StackTest
    {
        public static void LinkTest1()
        {
            var alphabetStack = new Stack<char>();
            alphabetStack.Push('A');
            alphabetStack.Push('B');
            alphabetStack.Push('C');

            Console.Write("first:");
            //使用迭代器相当于使用Peek()，不会删除栈中的元素
            foreach (var alphabet in alphabetStack)
            {
                Console.Write(alphabet);
            }
            Console.WriteLine();

            Console.Write("Second:");
            //Pop()返回并删除栈中元素
            while (alphabetStack.Count > 0)
            {
                Console.Write(alphabetStack.Pop());
            }
            Console.WriteLine();

        }
    }

}
