using System;
using System.Collections.Generic;

namespace 练习.DataStructrue.Stack
{
    public class HanoiTest
    {
        private Stack<string> stackA = new Stack<string>();
        private Stack<string> stackB = new Stack<string>();
        private Stack<string> stackC = new Stack<string>();

        public void hanoiTest()
        {
            for (int i = 0; i < 100; i++)
            {
                stackA.Push(i.ToString());
            }
            Hanoi(stackA.Count, stackA, stackB, stackC);
        }

        public void Hanoi(int n, Stack<string> a, Stack<string> b, Stack<string> c)
        {
            if (n == 1)
            {
                Move(a, c);
            }
            else
            {
                Hanoi(n - 1, a, c, b);
                Move(a, c);
                Hanoi(n - 1, b, a, c);
            }
        }

        public void Move(Stack<string> source, Stack<string> item)
        {
            try
            {
                var c = source.Pop();
                item.Push(c);
                WriteAll();
            }
            catch (Exception e)
            {
                Console.WriteLine("堆栈为空");
                //throw;
            }

        }

        public void WriteAll()
        {
            foreach (var s in stackA)
            {
                Console.WriteLine("A:" + s);
            }
            foreach (var s in stackB)
            {
                Console.WriteLine("B:" + s);
            }
            foreach (var s in stackC)
            {
                Console.WriteLine("C:" + s);
            }
            Console.WriteLine("-------------------------------------------");
        }
    }
}