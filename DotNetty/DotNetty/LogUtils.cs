using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetty
{
    class LogUtils
    {
        public static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
        public static void LogThreadInfo(string extmsg = "")
        {
            Console.WriteLine("==========" + extmsg + "============ Thread Id=" + Thread.CurrentThread.ManagedThreadId);
        }
    }
}
