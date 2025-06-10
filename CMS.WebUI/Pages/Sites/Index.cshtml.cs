using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Application.Services;
using CMS.Application.DTOs;

namespace CMS.WebUI.Pages.Sites
{
    public class IndexModel : PageModel
    {
        private readonly SiteService _siteService;
        private readonly ILogger<IndexModel> _logger;


        public IndexModel(SiteService siteService, ILogger<IndexModel> logger)
        {
            _siteService = siteService;
            _logger = logger;
        }

        public IList<Site> Sites { get;set; } = new List<Site>();

        public async Task OnGetAsync()
        {
            try
            {
                var sites = await _siteService.GetSitesAsync();
                if (sites != null)
                {
                    Sites = sites;
                }
                else
                {
                    _logger.LogWarning("No sites were returned by the service.");
                    Sites = new List<Site>(); // Ensure Sites is not null
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching sites.");
                Sites = new List<Site>(); // Ensure Sites is not null in case of error
                // Optionally, set an error message to display to the user
                // TempData["ErrorMessage"] = "Could not load sites.";
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var response = await _siteService.DeleteSiteAsync(id);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Site deleted successfully.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error deleting site {id}. Status: {response.StatusCode}. Content: {errorContent}");
                    TempData["ErrorMessage"] = $"Error deleting site: {response.ReasonPhrase} - {errorContent}";
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred while deleting site {id}.");
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the site.";
            }
            return RedirectToPage();
        }
    }
}
