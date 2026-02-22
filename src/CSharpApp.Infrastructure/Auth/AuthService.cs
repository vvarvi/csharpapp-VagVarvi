using CSharpApp.Core.Common;
using Microsoft.Extensions.Options;
using System.Net;
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

        public async Task<Result<AuthTokenResponse>> LoginAsync()
        {
            var payload = new
            {
                email = _settings.Username,
                password = _settings.Password
            };

            var response = await _httpClient.PostAsJsonAsync(
                _settings.Auth,
                payload);

            //response.EnsureSuccessStatusCode();

            //return await response.Content.ReadFromJsonAsync<AuthTokenResponse>()
            //       ?? throw new InvalidOperationException("Auth failed");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Error error = new Error("401", "Unauthorized: Invalid refresh token", 401);
                return Result<AuthTokenResponse>.Failure(error);
            }

            if (!response.IsSuccessStatusCode)
            {
                Error error = new Error("Code 002", "Is NOT Success Status Code: Authentication service error", 002);
                return Result<AuthTokenResponse>.Failure(error);
            }

            var token = await response.Content.ReadFromJsonAsync<AuthTokenResponse>();

            if (token == null)
            {
                Error error = new Error("Code 001", "Token is NULL", 001);

                return Result<AuthTokenResponse>.Failure(error);
            }

            return Result<AuthTokenResponse>.Success(token);
        }

        public async Task<Result<AuthTokenResponse>> RefreshAsync(string refreshToken)
        {
            var payload = new { refreshToken };

            var response = await _httpClient.PostAsJsonAsync(
                _settings.Refresh,
                payload);

            /*  Custom handling for 401 error response
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Invalid refresh token");
            */

            /*  Explicit - Fail-fast - Production - safe Null-safe    
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AuthTokenResponse>()
                            ?? throw new InvalidOperationException("Refresh Token response was null.");
            */

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Error error = new Error("401", "Unauthorized: Invalid refresh token", 401);
                return Result<AuthTokenResponse>.Failure(error);
            }

            if (!response.IsSuccessStatusCode)
            {
                Error error = new Error("Code 002", "Is NOT Success Status Code: Authentication service error", 002);
                return Result<AuthTokenResponse>.Failure(error);
            }

            var token = await response.Content.ReadFromJsonAsync<AuthTokenResponse>();

            if (token == null)
            {
                Error error = new Error("Code 001", "Token is NULL", 001);

                return Result<AuthTokenResponse>.Failure(error);
            }

            return Result<AuthTokenResponse>.Success(token);
        }
    }
}
