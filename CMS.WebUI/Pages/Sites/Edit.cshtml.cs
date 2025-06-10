using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CMS.Application.Services;
using CMS.Application.DTOs;
using System.Threading.Tasks;
using System.Linq; // Required for SelectList and .Any()
using System.Collections.Generic; // Required for List

namespace CMS.WebUI.Pages.Sites
{
    public class EditModel : PageModel
    {
        private readonly SiteService _siteService;
        private readonly ILogger<EditModel> _logger;

        public EditModel(SiteService siteService, ILogger<EditModel> logger)
        {
            _siteService = siteService;
            _logger = logger;
        }

        [BindProperty]
        public Site Site { get; set; } = new Site();

        public SelectList CompanyList { get; set; }

        private async Task LoadCompanyListAsync()
        {
            var companies = await _siteService.GetCompaniesAsync() ?? new List<Company>();
            CompanyList = new SelectList(companies, "Id", "Name", Site?.CompanyId);
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var site = await _siteService.GetSiteByIdAsync(id);
                if (site == null)
                {
                    _logger.LogWarning($"Site with ID {id} not found for editing.");
                    TempData["ErrorMessage"] = "Site not found.";
                    return RedirectToPage("./Index");
                }
                Site = site;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error fetching site with ID {id}.");
                TempData["ErrorMessage"] = "Error loading site data.";
                return RedirectToPage("./Index");
            }

            await LoadCompanyListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid for Edit Site.");
                await LoadCompanyListAsync(); // Reload company list if validation fails
                return Page();
            }

            try
            {
                var response = await _siteService.UpdateSiteAsync(Site.Id, Site);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Site updated successfully.";
                    return RedirectToPage("./Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error updating site {Site.Id}. Status: {response.StatusCode}. Content: {errorContent}");
                    ModelState.AddModelError(string.Empty, $"An error occurred while updating the site: {response.ReasonPhrase} - {errorContent}");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred while updating site {Site.Id}.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating the site.");
            }

            await LoadCompanyListAsync(); // Reload company list if there's an error
            return Page();
        }
    }
}
