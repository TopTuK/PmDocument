using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PmHelper.Domain.Repository.Entities
{
    [Table("DocumentRule")]
    public class DbDocumentRule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public byte AppendType { get; set; }

        [Required]
        public string? RuleText { get; set; }

        public byte Priority { get; set; }
    }
}
