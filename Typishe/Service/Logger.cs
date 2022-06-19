using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typishe.Service
{
    public static class Logger
    {
        private static List<string> _logs = new List<string>();

        public static void Add(string processName, string status)
        {
            var message = $"{_logs.Count + 1} - {DateTime.Now} - {processName}: {status}";
            _logs.Add(message);
        }
    }
}
