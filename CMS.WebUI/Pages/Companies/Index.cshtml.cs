using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Application.Services;
using CMS.Application.DTOs; // Ensure this is the correct namespace for Company DTO
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CMS.WebUI.Pages.Companies
{
    public class IndexModel : PageModel
    {
        private readonly CompanyService _companyService;

        public IndexModel(CompanyService companyService)
        {
            _companyService = companyService;
        }

        public IList<Company> Companies { get; set; } = new List<Company>();

        public async Task OnGetAsync()
        {
            var companiesList = await _companyService.GetCompaniesAsync();
            if (companiesList != null)
            {
                Companies = companiesList;
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var response = await _companyService.DeleteCompanyAsync(id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage(); // Refresh the page
            }
            // Handle error (e.g., display a message)
            // For now, just redirecting. A better approach would be to add an error message.
            ModelState.AddModelError(string.Empty, "Error deleting company. It might be in use or another issue occurred.");
            // Reload data if delete failed to show current state
            var companiesList = await _companyService.GetCompaniesAsync();
            if (companiesList != null)
            {
                Companies = companiesList;
            }
            return Page();
        }
    }
}
