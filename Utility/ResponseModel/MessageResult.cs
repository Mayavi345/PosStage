using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class MessageResult
    {
        public string Message { get; set; }
        public MessageType Type { get; set; }
    }

    public enum MessageType
    {
        Success,
        Error,
        Information,
        Warning
    }
}
