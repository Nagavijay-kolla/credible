using System;
using System.Collections.Generic;
using System.Text;

namespace CBH.ChatSignalR.Domain
{
    public class BroadcastMessage
    {
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
    }
}
