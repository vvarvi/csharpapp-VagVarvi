using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace CSharpApp.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        private readonly RestApiSettings _settings;

        public string? _cachedToken;

        public DateTime _tokenExpiration;

        public AuthService(HttpClient httpClient, IOptions<RestApiSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<AuthTokenResponse> LoginAsync()
        {
            var payload = new
            {
                email = _settings.Username,
                password = _settings.Password
            };

            var response = await _httpClient.PostAsJsonAsync(
                _settings.Auth,
                payload);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AuthTokenResponse>()
                   ?? throw new Exception("Auth failed");
        }

        public async Task<AuthTokenResponse> RefreshAsync(string refreshToken)
        {
            var payload = new { refreshToken };

            var response = await _httpClient.PostAsJsonAsync(
                _settings.Refresh,
                payload);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AuthTokenResponse>()
                   ?? throw new Exception("Refresh token failed");
        }
    }
}
