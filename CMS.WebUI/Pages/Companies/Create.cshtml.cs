using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Application.Services;
using CMS.Application.DTOs;
using System.Threading.Tasks;

namespace CMS.WebUI.Pages.Companies
{
    public class CreateModel : PageModel
    {
        private readonly CompanyService _companyService;

        public CreateModel(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [BindProperty]
        public Company Company { get; set; } = new Company();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _companyService.CreateCompanyAsync(Company);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                // You could read the error response from the API here
                ModelState.AddModelError(string.Empty, "An error occurred while creating the company.");
                return Page();
            }
        }
    }
}
