using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTest.ThreadTest
{
    public class TreadMain
    {
        public static void Write()
        {
            //AsynDelegate.AsynDelegateMain();
            //ThreadClass.ThreadClassMain();
            //TreadPoolTest.TreadPoolMain();
            //TaskTest.TaskTestMain();
            //ParallelTest.ParallelTestMain();
            ThreadSynchronous.ThreadSynchronousMain();
        }
    }
}
