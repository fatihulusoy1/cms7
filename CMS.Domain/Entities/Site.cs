using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Domain.Entities
{
    public class Site
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; } // Navigation property

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
