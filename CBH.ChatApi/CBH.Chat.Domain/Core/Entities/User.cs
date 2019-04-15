using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBH.Chat.Domain.Core.Entities
{
    public class User 
    {
        public Int16 Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public bool IsEnableChat { get; set; }

        [NotMapped]
        public bool IsAdmin { get; set; }
        public bool IsHighImportanceEnable { get; set; }
        public bool Deleted { set; get; }
        public string Status { get; set; }
        public bool IsEmployee { get; set; }
        public string ProfileCode { get; set; }
    }
}