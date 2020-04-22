using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading;
using NUnit.Framework;

namespace SampleCode.Test.Gather
{
    /// <summary>
    ///队列测试
    /// </summary>
    /// 先进先出的列表
    public class QueueTest
    {
        /// <summary>
        /// 队列基本操作
        /// </summary>
        [Test]
        public void QueueCodeTest()
        {
            //设置队列容量，默认容量为4，若容量不足进行翻倍递增。
            var queue = new Queue<Document>(10);

            //通过IEnumerable<T>初始化队列
            List<Document> ducuments = new List<Document>()
            {
                new Document("1", "111"),
                new Document("2", "222"),
            };
            queue = new Queue<Document>(ducuments);

            //入列，向队列插入元素
            queue.Enqueue(new Document("3", "333"));

            //出列，从队首获取元素并删除
            var ducument1 = queue.Dequeue();

            //出列，从对首获取元素不删除
            var ducument2 = queue.Peek();

            //重新设置队列容量，即将array中的空元素过滤，Dequeue不会重新设置队列容量
            queue.TrimExcess();
        }

        /// <summary>
        /// 多线程下的队列
        /// </summary>
        [Test]
        public static void QueueCodeTest1()
        {
            var dm = new DocumentManager();
            ProcessDocument.Start(dm);

            for (var i = 0; i < 1000; i++)
            {
                dm.AddDocument(new Document("Document" + i, "content"));
                Console.WriteLine($"Added document {"Document" + i}");
                Thread.Sleep(new Random().Next(20));
            }
        }
    }
    public class Document
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Document(string title, string content)
        {
            Content = content;
            Title = title;
        }
    }

    public class DocumentManager
    {
        private static readonly Queue<Document> QueueDocument = new Queue<Document>();

        public void AddDocument(Document document)
        {
            lock (this)
            {
                QueueDocument.Enqueue(document);
            }
        }

        public Document GetDocument()
        {
            lock (QueueDocument)
            {
                return QueueDocument.Dequeue();
            }
        }

        public bool IsDocumentAvailable => QueueDocument.Count > 0;

    }

    public class ProcessDocument
    {
        private static DocumentManager _documentManager;

        protected ProcessDocument(DocumentManager dm)
        {
            _documentManager = dm;
        }

        public static void Start(DocumentManager dm)
        {
            new Thread(new ProcessDocument(dm).Run).Start();
        }

        protected void Run()
        {
            while (true)
            {
                if (_documentManager.IsDocumentAvailable)
                {
                    var document = _documentManager.GetDocument();
                    Console.WriteLine($"Process document:{document.Title}");
                }
                Thread.Sleep(new Random().Next(200));
            }
        }
    }
}