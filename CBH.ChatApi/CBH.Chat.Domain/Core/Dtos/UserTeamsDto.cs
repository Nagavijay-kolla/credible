using System.Collections.Generic;
using CBH.Chat.Domain.Core.Entities;

namespace CBH.Chat.Domain.Core.Dtos
{
    public class UserTeamsDto
    {
        public int UserId { get; set; }
        public IList<Team> Teams {get;set;}
    }
}