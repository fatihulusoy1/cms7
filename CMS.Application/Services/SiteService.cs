using System.Net.Http;
using System.Net.Http.Json; // Requires System.Net.Http.Json package
using System.Threading.Tasks;
using System.Collections.Generic;
using CMS.Application.DTOs; // USE THIS NAMESPACE for Site and Company
using Microsoft.Extensions.Configuration;

namespace CMS.Application.Services
{
    // REMOVE local Site and Company class definitions that were here

    public class SiteService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public SiteService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Get the base URL from configuration, fallback to a default
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "https://localhost:7295/api";
        }

        public async Task<List<Site>> GetSitesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Site>>($"{_apiBaseUrl}/api/sites");
        }

        public async Task<Site> GetSiteByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Site>($"{_apiBaseUrl}/api/sites/{id}");
        }

        public async Task<HttpResponseMessage> CreateSiteAsync(Site site)
        {
            // The 'Company' navigation property should not be sent in the POST body
            // if the backend expects only CompanyId. Create a DTO or set Company to null.
            var siteToCreate = new { // Anonymous type or a DTO
                site.CompanyId,
                site.Name,
                site.Location,
                site.Description
            };
            return await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/api/sites", siteToCreate);
        }

        public async Task<HttpResponseMessage> UpdateSiteAsync(int id, Site site)
        {
            // Similar to Create, send only necessary data.
            var siteToUpdate = new {
                site.Id, // Usually needed for backend validation (id in body vs id in URL)
                site.CompanyId,
                site.Name,
                site.Location,
                site.Description
            };
            return await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}/api/sites/{id}", siteToUpdate);
        }

        public async Task<HttpResponseMessage> DeleteSiteAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/sites/{id}");
        }

        // Method to get companies for the dropdown in Create/Edit site forms
        public async Task<List<Company>> GetCompaniesAsync()
        {
            // Assuming you have a CompanyService or direct access to companies API
            // This might need to call a different endpoint or use CompanyService
            // For simplicity, let's assume a direct endpoint, or this could be
            // handled by reusing CompanyService if it's registered.
            // If CompanyService is available and registered, it's better to inject it
            // and use it here. For now, direct call:
            return await _httpClient.GetFromJsonAsync<List<Company>>($"{_apiBaseUrl}/api/companies");
        }
    }
}
