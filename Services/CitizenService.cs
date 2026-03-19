using System.Text.Json;
using CitizensWebApi.Models;
using CitizensWebApi.Models.DTOs;

namespace CitizensWebApi.Services
{
    public class CitizenService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CitizenService> _logger;
        private readonly string _apiUrl;

        public CitizenService(HttpClient httpClient, IConfiguration configuration, ILogger<CitizenService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiUrl = configuration.GetValue<string>("ExternalApi:Url") ?? "https://api.restful-api.dev/objects";
        }

        public async Task<Citizen> AssembleCitizenAsync(CreateCitizenDto dto)
        {
            return new Citizen
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CI = dto.CI,
                BloodGroup = GetRandomBloodGroup(),
                PersonalAsset = await GetRandomPersonalAssetAsync()
            };
        }

        private string GetRandomBloodGroup()
        {
            string[] bloodGroups = { "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-" };
            return bloodGroups[new Random().Next(bloodGroups.Length)];
        }

        private async Task<string> GetRandomPersonalAssetAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("External API request executed");
                    var jsonString = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(jsonString);
                    var root = doc.RootElement;

                    if (root.GetArrayLength() > 0)
                    {
                        var randomElement = root[new Random().Next(root.GetArrayLength())];
                        if (randomElement.TryGetProperty("name", out var nameProp))
                        {
                            return nameProp.GetString() ?? "Default Asset";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "External API request failed");
            }
            return "Default Asset";
        }
    }
}