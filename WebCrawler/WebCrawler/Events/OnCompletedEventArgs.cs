﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Events
{
    public class OnCompletedEventArgs
    {
        public Uri Uri { get; }// 爬虫URL地址

        public int ThreadId { get; }// 任务线程ID

        public string PageSource { get; }// 页面源代码

        public byte[] Bitmap { get; } //图片信息

        public long Milliseconds { get; }// 爬虫请求执行事件

        public Dictionary<string,string> SpecialArguments { get; set; }

        public OnCompletedEventArgs(Uri uri, int threadId, long milliseconds, string pageSource, byte[] bitmap, Dictionary<string, string> specialArguments = null)
        {
            Uri = uri;
            ThreadId = threadId;
            Milliseconds = milliseconds;
            PageSource = pageSource;
            Bitmap = bitmap;
            SpecialArguments = specialArguments;
        }
    }
}
