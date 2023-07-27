using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PmHelper.Domain.Repository.Entities
{
    [Table("DocumentType")]
    public class DbDocumentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;


        [Required]
        public string Prompt { get; set; } = string.Empty;

        [Required]
        public string AssistantName { get; set; } = string.Empty;

        [Required]
        public byte ResultFormat { get; set; }
    }
}
