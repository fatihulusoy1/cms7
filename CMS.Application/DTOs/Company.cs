 using System.ComponentModel.DataAnnotations;

namespace CMS.Application.DTOs
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Add other properties if they are used by the Company pages
        // For the Site's purpose, Id and Name are primary.
        [StringLength(250)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? TaxCode { get; set; }

        [StringLength(100)]
        public string? TaxOffice { get; set; }
    }
}
