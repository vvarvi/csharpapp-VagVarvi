using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpApp.Infrastructure.Auth
{
    public interface ITokenProvider
    {
        Task<string> GetAccessTokenAsync();
    }
}
