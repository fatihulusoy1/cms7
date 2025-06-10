using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CMS.Application.Services;
using CMS.Application.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CMS.WebUI.Pages.Sites
{
    public class CreateModel : PageModel
    {
        private readonly SiteService _siteService;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(SiteService siteService, ILogger<CreateModel> logger)
        {
            _siteService = siteService;
            _logger = logger;
        }

        [BindProperty]
        public Site Site { get; set; } = new Site();

        public SelectList CompanyList { get; set; } = new SelectList(new List<Company>(), "Id", "Name");

        // Sayfa ilk y�klendi�inde �a�r�l�r, �irket listesini y�kler
        public async Task OnGetAsync()
        {
            await LoadCompanyListAsync();
        }

        // �irket listesini API'den �ekip dropdown i�in haz�rlar
        private async Task LoadCompanyListAsync()
        {
            var companies = await _siteService.GetCompaniesAsync() ?? new List<Company>();
            CompanyList = new SelectList(companies, "Id", "Name");
        }

        // Form g�nderildi�inde �a�r�l�r
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid.");
                await LoadCompanyListAsync(); // Dropdown'u yeniden y�kle
                return Page();
            }

            try
            {
                var response = await _siteService.CreateSiteAsync(Site);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Site created successfully.";
                    return RedirectToPage("./Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error creating site: {response.StatusCode} - {errorContent}");
                    ModelState.AddModelError(string.Empty, $"Failed to create site: {response.ReasonPhrase} - {errorContent}");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Exception while creating site.");
                ModelState.AddModelError(string.Empty, "Unexpected error occurred.");
            }

            await LoadCompanyListAsync(); // Hata durumunda dropdown'u tekrar y�kle
            return Page();
        }
    }
}
