using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Infrastructure.Auth
{
    public interface IAuthService
    {
        Task<AuthTokenResponse> LoginAsync();
        Task<AuthTokenResponse> RefreshAsync(string refreshToken);
    }
}
