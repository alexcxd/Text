using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Model;

namespace WebCrawler
{
    public class NameToUrlManage
    {
        private Queue<NameToUrl> items;
        private object objlock = new object();

        public NameToUrlManage(Queue<NameToUrl> urls)
        {
            this.items = urls;
        }

        public NameToUrl GetUrl()
        {
            lock (objlock)
            {
                if (items == null || items.Count == 0)
                {
                    throw new ArgumentNullException();
                }
                return items.Dequeue();
            }
        }
    }
}
