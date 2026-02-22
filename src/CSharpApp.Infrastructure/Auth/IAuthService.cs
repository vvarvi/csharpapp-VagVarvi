using CSharpApp.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Infrastructure.Auth
{
    public interface IAuthService
    {
        Task<Result<AuthTokenResponse>> LoginAsync();
        Task<Result<AuthTokenResponse>> RefreshAsync(string refreshToken);
    }
}
