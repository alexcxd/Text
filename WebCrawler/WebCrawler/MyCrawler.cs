using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Events;

namespace WebCrawler
{
    class MyCrawler : ICrawler
    {
        public event EventHandler<OnStartEventArgs> OnStart;
        public event EventHandler<OnCompletedEventArgs> OnCompleted;
        public event EventHandler<OnErrorEventArgs> OnError;

        public CookieContainer CookieContainer { get; set; }

        public MyCrawler() { }

        public async Task<string> Start(Uri uri, string proxy = null)
        {
            return await Task.Run<string>(() =>
            {
                OnStart?.Invoke(this, new OnStartEventArgs(uri));

                try
                {
                    var request = WebRequest.Create(uri) as HttpWebRequest;
                    request.Accept = "*/*";
                }
                catch (Exception e)
                {

                }
                finally
                {

                }
                var a = "";
                return a;
            });
        }
    }

}
