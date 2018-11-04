using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 练习.DataStructrue.tree
{
    class TreeMain
    {

        public static void Write()
        {
            //二叉搜索树
            BinaryTreeArray treeArray;
            BinarySortTreeLink treeLink;
            List<int> datas = new List<int>() { 5, 9, 1, 6, 4, 8, 2, 7, 3 };

            //输入
            /*int i = 0;
            while (true)
            {
                string readStr = Console.ReadLine();
                if (readStr.Equals("0"))
                {
                    break;
                }
                datas.Add(Int32.Parse(readStr));
                i++;
            }*/

            //treeArray = new BinaryTreeArray(datas);
            treeLink = new BinarySortTreeLink(datas);

            //查找值
            /*Console.Write("请输入要查找的值：");
            int value = Int32.Parse(Console.ReadLine());
            BinaryTreeNodeModel node = treeLink.SelectLinkTree(value);
            if (node == null) Console.WriteLine("该值在树中不存在");
            else Console.WriteLine("该值在树中存在，他的父亲节点为" + node.parent.value);*/

            //删除值
            Console.Write("请输入要删除的值：");
            int value = Int32.Parse(Console.ReadLine());
            string result = treeLink.DeleteLinkTree(value);
            Console.WriteLine(result);
        }

    }
}
