using System.Collections.Generic;

namespace DotNetTest.DataStructrue
{
    /// <summary>
    /// 二叉搜索树
    /// </summary>
    public class BinarySortTreeLink
    {
        public BinaryTreeNodeModel Root = new BinaryTreeNodeModel();
        public BinarySortTreeLink() { }

        /// <summary>
        /// 初始化树
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>批量加入节点</remarks>
        /// <returns></returns>
        public BinarySortTreeLink(List<int> datas)
        {
            if (datas.Count > 0)
            {
                Root.value = datas[0];
                Root.parent = null;
                for (int i = 1; i < datas.Count; i++)
                {
                    AddLinkTree(Root, datas[i]);
                }
            }
        }

        /// <summary>
        /// 在树上添加节点（递归实现）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object AddLinkTree(BinaryTreeNodeModel root, int curr)
        {
            if (curr >= root.value)
            {
                if (root.childR != null)
                {
                    return AddLinkTree(root.childR, curr);
                }
                BinaryTreeNodeModel child = new BinaryTreeNodeModel(curr, root);
                root.childR = child;
            }
            else
            {
                if (root.childL != null)
                {
                    return AddLinkTree(root.childL, curr);
                }
                BinaryTreeNodeModel child = new BinaryTreeNodeModel(curr, root);
                root.childL = child;
            }
            return null;
        }

        /// <summary>
        /// 在树上查找节点（非递归实现）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BinaryTreeNodeModel SelectLinkTree(int curr)
        {
            BinaryTreeNodeModel currNode = Root;
            while (true)
            {
                if (curr > currNode.value)
                {
                    currNode = currNode.childR;
                }
                else if (curr < currNode.value)
                {
                    currNode = currNode.childL;
                }
                else
                {
                    return currNode;
                }
                //如果当前的父节点为空说明遍历到底了
                if (currNode == null)
                {
                    break;
                }
            }
            return null;
        }

        /// <summary>
        /// 在树上删除节点
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string DeleteLinkTree(int curr)
        {
            BinaryTreeNodeModel node = SelectLinkTree(curr);
            if (node == null)
            {
                return "该节点不存在";
            }

            if (node.childR == null && node.childL == null)
            {
                if (node == Root)
                {
                    Root = null;
                }
                else
                {
                    BinaryTreeNodeModel parent = node.parent;
                    if (parent.childL != null && parent.childL.value == node.value)
                    {
                        parent.childL = null;
                    }
                    else
                    {
                        parent.childR = null;
                    }
                }
            }
            else if ((node.childR != null && node.childL == null) || (node.childR == null && node.childL != null))
            {
                if (node == Root)
                {
                    Root = node.childR ?? node.childL;
                }
                else
                {

                    BinaryTreeNodeModel parent = node.parent;
                    if (parent.childL != null && parent.childL.value == node.value)
                    {
                        parent.childL = node.childR ?? node.childL;
                    }
                    else
                    {
                        parent.childR = node.childR ?? node.childL;
                    }
                }
            }
            else
            {
                BinaryTreeNodeModel childLMax = FindLeftTreeMax(Root);
                BinaryTreeNodeModel temp;
                BinaryTreeNodeModel parent;
                if (node == Root)
                {
                    temp = Root;
                    Root = childLMax;
                    childLMax.childR = temp.childR;
                    childLMax.childL = temp.childL;
                }
                else
                {
                    parent = node.parent;
                    if (parent.childL.value == childLMax.value)
                    {
                        parent.childL = childLMax;
                    }
                    else
                    {
                        parent.childR = childLMax;
                    }
                    childLMax.childL = node.childL;
                    childLMax.childR = node.childR;
                }

                //将childLMax删除
                parent = childLMax.parent;
                if (parent.childL != null && parent.childL.value == childLMax.value)
                {
                    parent.childL = null;
                }
                else
                {
                    parent.childR = null;
                }
            }
            return "删除成功";
        }

        /// <summary>
        /// 查找当前节点左子树的最大值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BinaryTreeNodeModel FindLeftTreeMax(BinaryTreeNodeModel currNode)
        {
            BinaryTreeNodeModel node = currNode.childL;

            if(node == null)
            {
                return null;
            }

            while (true)
            {
                if (node.childR == null)
                {
                    return node;
                }
                node = node.childR;
            }
        }
    }

    public class BinaryTreeNodeModel
    {
        public int value { get; set; }

        public BinaryTreeNodeModel childL { get; set; }

        public BinaryTreeNodeModel childR { get; set; }

        public BinaryTreeNodeModel parent { get; set; }

        public BinaryTreeNodeModel() { }

        public BinaryTreeNodeModel(int value, BinaryTreeNodeModel parent)
        {
            this.value = value;
            this.childL = null;
            this.childR = null;
            this.parent = parent;
        }
    }
}