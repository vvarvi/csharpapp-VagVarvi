using CSharpApp.Core.Common;
using CSharpApp.Infrastructure.Security.Jwt;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace CSharpApp.Infrastructure.Auth
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IAuthService _authService;

        private readonly JwtOptions _jwtOptions;

        private string? _accessToken;
        
        private string? _refreshToken;
        
        private DateTime _expiry;

        public TokenProvider(IAuthService authService, IOptions<JwtOptions> jwtOptions)
        {
            _authService = authService;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (_accessToken == null || DateTime.UtcNow >= _expiry)
            {
                //var auth = _refreshToken == null
                //    ? await _authService.LoginAsync()
                //    : await _authService.RefreshAsync(_refreshToken);

                //_accessToken = auth.AccessToken;
                //_refreshToken = auth.RefreshToken;

                ////_expiry = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes); // or parse JWT exp
                //_expiry = GetExpiryFromJwt(auth.AccessToken).AddSeconds(-_jwtOptions.ExpirationBufferSeconds);

                Result<AuthTokenResponse> auth = _refreshToken == null
                    ? await _authService.LoginAsync()
                    : await _authService.RefreshAsync(_refreshToken);

                if (auth.Value == null)
                {
                    throw new InvalidOperationException("Authentication failed: token response is null.");
                }

                _accessToken = auth.Value.AccessToken;
                _refreshToken = auth.Value.RefreshToken;

                _expiry = GetExpiryFromJwt(_accessToken!).AddSeconds(-_jwtOptions.ExpirationBufferSeconds);
            }

            return _accessToken!;
        }

        private DateTime GetExpiryFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var expClaim = token.Claims.First(c => c.Type == "exp").Value;

            var expiry = DateTimeOffset
                .FromUnixTimeSeconds(long.Parse(expClaim))
                .UtcDateTime;

            return expiry;
        }

    }
}
