using System.ComponentModel.DataAnnotations;

namespace CMS.Application.DTOs
{
    public class Site
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public Company? Company { get; set; } // Navigation property for displaying company name

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
