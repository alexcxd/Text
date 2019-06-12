using System.Collections.Generic;
using DotNetTest.DataStructrue.LinkList;

namespace DotNetTest.Gather
{
    public class LinkedListTest
    {
        public static void LinkedListTest1()
        {
            //双向链表
            var linkedList = new LinkedList<string>();

            //插入节点的方法
            var aaa = linkedList.AddFirst("AAA");
            var zzz = linkedList.AddLast("ZZZ");
            var bbb = linkedList.AddAfter(aaa, "BBB");
            var yyy = linkedList.AddBefore(zzz, "YYY");

            //移除节点的方法
            linkedList.RemoveFirst();
            linkedList.RemoveLast();
            linkedList.Remove("ZZZ");

            //查找节点的方法
            var bbbFind = linkedList.Find("BBB");
            var bbbFindLast = linkedList.FindLast("BBB");
        }   
    }
}