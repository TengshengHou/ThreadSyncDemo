using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterlockStructure
{
    public sealed class MultiWebRequests
    {
        AsyncCoordinator m_ac = new AsyncCoordinator();
        private Dictionary<string, object> m_servers = new Dictionary<string, object>()
        {
            { "https://www.baidu.com/",null},
            { "https://www.cnblogs.com/",null},
            { "https://1.1.1.1/",null},
        };
        public MultiWebRequests(Int32 timeout = Timeout.Infinite)
        {
            var httpClient = new HttpClient();
            foreach (var server in m_servers.Keys)
            {
                m_ac.AboutToBegin(1);
                httpClient.GetByteArrayAsync(server).
                    ContinueWith(task =>
                    {
                        ComputeResult(server, task);
                    });
            }

            m_ac.AllBegun(AllDone, timeout);
        }
        private void ComputeResult(string server, Task<byte[]> task)
        {
            Console.WriteLine("ComputeResult  当前线程ID {0}"+ server, Thread.CurrentThread.ManagedThreadId);
            object result;
            if (task.Exception != null)
            {
                result = task.Exception.InnerException;
            }
            else
            {
                //在线程池上处理I/O完成
                result = task.Result.Length;
            }

            m_servers[server] = result;
            m_ac.JustEnded();
        }

        public void Cancel() { m_ac.Cancel(); }
        private void AllDone(CoordinationStatus status)
        {
            Console.WriteLine("MultiWebRequests.AllDone 当前线程ID {0}", Thread.CurrentThread.ManagedThreadId);
            switch (status)
            {
                case CoordinationStatus.Cancel:
                    Console.WriteLine("CoordinationStatus.Cancel 当前线程ID {0}", Thread.CurrentThread.ManagedThreadId);
                    break;
                case CoordinationStatus.Timeout:
                    Console.WriteLine("CoordinationStatus.Timeout 当前线程ID {0}", Thread.CurrentThread.ManagedThreadId);
                    break;
                case CoordinationStatus.AllDone:
                    Console.WriteLine("CoordinationStatus.AllDone 当前线程ID {0}", Thread.CurrentThread.ManagedThreadId);
                    break;
                default:
                    break;
            }

            Console.WriteLine("结束    当前线程ID {0}", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
