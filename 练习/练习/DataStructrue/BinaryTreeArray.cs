

using System;
using System.Collections;
using System.Collections.Generic;

namespace 练习.DataStructrue
{
    class BinaryTreeArray
    {
        public BinaryTreeArray() { }

        /// <summary>
        /// 初始化树
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>批量加入节点</remarks>
        /// <returns></returns>
        public BinaryTreeArray(List<int> datas)
        {
            int[] tree = new int[10000];
            tree[1] = datas[0]; //根节点     
            //二叉查找树 ,中序遍历可以得到该数组的递增排序
            for (int i = 1; i < datas.Count; i++)
            {
                AddArray(tree, datas[i], 1);
            }
        }

        /// <summary>
        /// 在树上添加节点（递归实现）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object AddArray(int[] tree, int curr, int node)
        {
            if (curr >= tree[node])
            {
                if (tree[2 * node + 1] == 0)
                {
                    tree[2 * node + 1] = curr;
                }
                else
                {
                    return AddArray(tree, curr, 2 * node + 1);
                }
            }
            else
            {
                if (tree[2 * node] == 0)
                {
                    tree[2 * node] = curr;
                }
                else
                {
                    return AddArray(tree, curr, 2 * node);
                }
            }
            return null;
        }


        //冒泡排序
        public void Sort(List<int> datas)
        {
            for (int i = 0; i < datas.Count; ++i)
            {
                for (int j = 0; j < datas.Count - i -1; j++)
                {
                    if (datas[j] > datas[j + 1])
                    {
                        int temp = datas[j];
                        datas[j] = datas[j + 1];
                        datas[j + 1] = temp;
                    }
                }
            }
        }

    }

    

    
}
