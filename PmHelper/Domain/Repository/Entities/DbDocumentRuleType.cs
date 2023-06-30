using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace PmHelper.Domain.Repository.Entities
{
    [Table("DocumentRuleType")]
    public class DbDocumentRuleType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RuleId { get; set; }

        public int TypeId { get; set; }

        [ForeignKey(nameof(RuleId))]
        public DbDocumentRule? DocumentRule { get; set; }


        [ForeignKey(nameof(TypeId))]
        public DbDocumentType? DocumentType { get; set; }
    }
}
