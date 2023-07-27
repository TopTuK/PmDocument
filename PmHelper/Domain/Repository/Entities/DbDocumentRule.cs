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

        public byte AppendType { get; set; } = 0;

        [Required]
        public string RuleText { get; set; } = string.Empty;

        public byte Priority { get; set; } = 0;
    }
}
