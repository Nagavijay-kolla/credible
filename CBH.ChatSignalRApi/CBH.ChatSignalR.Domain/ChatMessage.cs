using System;
using System.Collections.Generic;

namespace CBH.ChatSignalR.Domain
{
    public class ChatMessage
    {
        public ChatMessage()
        {
            Readby = new List<ReadByUsers>();
        }
        public Guid ThreadID { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        public bool? IsHighImportance { get; set; }
        public bool? IsArchived { get; set; }
        public ChatType? ChatType { get; set; }
        public string TeamOrGroupName { get; set; }
        public Guid? GroupId { get; set; }
        public int? Team { get; set; }
        public List<ReadByUsers> Readby {get;set;}
        public bool? Refresh { get; set; }
        
    }
    public class ReadByUsers
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime ReadAt{get;set;}
    }
}
