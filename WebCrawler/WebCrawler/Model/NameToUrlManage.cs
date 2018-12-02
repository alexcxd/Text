using System;
using System.Collections.Generic;

namespace WebCrawler.Model
{
    public class NameToUrlManage
    {
        private Queue<NameToUrl> items;
        private object objlock = new object();

        public int Count => items.Count;

        public NameToUrlManage()
        {
            items = new Queue<NameToUrl>();
        }

        public NameToUrlManage(Queue<NameToUrl> urls)
        {
            items = urls;
        }

        public NameToUrl GetUrl()
        {
            if (items == null || items.Count == 0)
            {
                throw new ArgumentNullException();
            }
            lock (objlock)
            {
                return items.Dequeue();
            }
        }

        public void SetId(NameToUrl nameToUrl)
        {
            lock (objlock)
            {
                items.Enqueue(nameToUrl);
            }
        }
    }
}
