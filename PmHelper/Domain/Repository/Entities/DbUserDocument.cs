using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PmHelper.Domain.Repository.Entities
{
    [Table("UserDocument")]
    public class DbUserDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int TypeId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public DateTime EditedDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public DbUser? UserProfile { get; set; }

        [ForeignKey(nameof(TypeId))]
        public DbDocumentType? DocumentType { get; set; }
    }
}
