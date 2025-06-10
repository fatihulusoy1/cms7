using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Application.Services;
using CMS.Application.DTOs;
using System.Threading.Tasks;

namespace CMS.WebUI.Pages.Companies
{
    public class EditModel : PageModel
    {
        private readonly CompanyService _companyService;

        public EditModel(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [BindProperty]
        public Company Company { get; set; } = new Company();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            Company = company;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _companyService.UpdateCompanyAsync(Company.Id, Company);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the company.");
                return Page();
            }
        }
    }
}
