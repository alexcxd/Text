using DotNetTest.DataStructrue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleCode.Test.DataStructrue.Tree
{
    [TestFixture]
    public class BinarySortTreeLinkTest
    {
        private BinarySortTreeLink treeLink;

        [SetUp]
        public void SetUp()
        {
            List<int> datas = new List<int>() { 5, 9, 1, 6, 4, 8, 2, 7, 3 };
            treeLink = new BinarySortTreeLink(datas);
        }

        [Test]
        public void AddLinkTreeTest()
        {
            treeLink.AddLinkTree(treeLink.Root, 10);
            var node = treeLink.SelectLinkTree(10);
            Console.WriteLine(node.value);
        }

        [Test]
        [TestCase(1)]
        [TestCase(13)]
        public void SelectLinkTreeTest(int value)
        {
            var node = treeLink.SelectLinkTree(value);

            if(node == null)
            {
                Console.WriteLine("该值在树中不存在");
            }
            else
            {
                Console.WriteLine("该值在树中存在，他的父亲节点为" + node.parent.value);
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(13)]
        public void DeleteLinkTreeTest(int value)
        {
            string result = treeLink.DeleteLinkTree(value);
            Console.WriteLine(result);
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void FindLeftTreeMaxTest(int value)
        {
            var node = treeLink.SelectLinkTree(value);
            var result = treeLink.FindLeftTreeMax(node);
            if (result == null)
            {
                Console.WriteLine("左子树为空");
            }
            else
            {
                Console.WriteLine(result.value);
            }
        }
    }
}
