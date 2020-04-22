namespace SampleCode.DataStructrue
{
    /// <summary>
    /// 节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Node<T>
    {
        public T Value { get; set; }

        public Node<T> NextNode { get; set; }
    }

    class LinkList<T>
    {
        /// <summary>
        /// 头结点
        /// </summary>
        private static Node<T> root;

        public Node<T> nodeEnd;

        public LinkList()
        {
            root = new Node<T>();
            root.NextNode = null;
            nodeEnd = root;
        }

        /// <summary>
        /// 添加一个节点到链表尾部
        /// </summary>
        /// <param name="element"></param>
        public void Add(T element)
        {
            //初始化出一个节点
            var node = new Node<T>();
            node.Value = element;
            node.NextNode = null;

            //将节点加入链表，并且更新nodeEnd
            nodeEnd.NextNode = node;
            nodeEnd = node;
        }

        /// <summary>
        /// 移除最后一个节点
        /// </summary>
        public void Remove()
        {

        }
    }
}
