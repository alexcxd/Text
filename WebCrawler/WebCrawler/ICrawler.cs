using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Events;

namespace WebCrawler
{
    public interface ICrawler
    {
        event EventHandler<OnStartEventArgs> OnStart;//爬虫启动事件

        event EventHandler<OnCompletedEventArgs> OnCompleted;//爬虫完成事件

        event EventHandler<OnErrorEventArgs> OnError;//爬虫出错事件

        Task<string> Start(Uri uri, string refererUrl, Dictionary<string, string> specialArguments = null); //异步爬虫


    }
}
