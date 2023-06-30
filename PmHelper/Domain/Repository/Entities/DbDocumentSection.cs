using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PmHelper.Domain.Repository.Entities
{
    [Table("DocumentSection")]
    public class DbDocumentSection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TypeId { get; set; }

        [Required]
        public string? Title { get; set; }

        public byte Priority { get; set; } = 0;

        [ForeignKey(nameof(TypeId))]
        public DbDocumentType? DocumentType { get; set; }
    }
}
