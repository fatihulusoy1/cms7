// CMS/Models/Company.cs
using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string TaxCode { get; set; }

        [StringLength(100)]
        public string TaxOffice { get; set; }
    }
}
