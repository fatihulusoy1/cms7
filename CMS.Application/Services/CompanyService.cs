using System.Net.Http;
using System.Net.Http.Json; // Requires System.Net.Http.Json package
using System.Threading.Tasks;
using System.Collections.Generic;
using CMS.Application.DTOs; // Assuming Company model is here
using Microsoft.Extensions.Configuration; // For IConfiguration

namespace CMS.Application.Services
{
    public class CompanyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public CompanyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // It's good practice to get the base URL from configuration
            // Fallback to launchSettings derived URL if not in config.
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "https://localhost:7295/api";
        }

        public async Task<List<Company>> GetCompaniesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Company>>($"{_apiBaseUrl}/api/companies");
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Company>($"{_apiBaseUrl}/api/companies/{id}");
        }

        public async Task<HttpResponseMessage> CreateCompanyAsync(Company company)
        {
            return await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/api/companies", company);
        }

        public async Task<HttpResponseMessage> UpdateCompanyAsync(int id, Company company)
        {
            return await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}/api/companies/{id}", company);
        }

        public async Task<HttpResponseMessage> DeleteCompanyAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/companies/{id}");
        }
    }
}
