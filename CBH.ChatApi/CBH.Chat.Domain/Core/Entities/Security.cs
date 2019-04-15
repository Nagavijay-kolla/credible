using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CBH.Chat.Domain.Core.Entities
{
    [Table("Security")]
    public class Security 
    {
        [Key]
        public int Security_Id { get; set; }
        public string Action { get; set; }
    }
}