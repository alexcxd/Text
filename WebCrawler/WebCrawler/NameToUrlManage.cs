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

        public NameToUrlManage(ICollection<NameToUrl> urls)
        {
            this.items = urls as Queue<NameToUrl>;
        }

        public NameToUrl GetUrl()
        {
            lock (objlock)
            {
                if (items == null || items.Count == 0)
                {
                    throw new ArgumentNullException("未初始化或者队列已为空");
                }
                return items.Dequeue();
            }
        }
    }
}
