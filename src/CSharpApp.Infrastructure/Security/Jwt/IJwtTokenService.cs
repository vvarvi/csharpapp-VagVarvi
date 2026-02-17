using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Infrastructure.Security.Jwt
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string role);
    }
}
