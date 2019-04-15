using System.ComponentModel.DataAnnotations;

namespace CBH.Chat.Domain.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}