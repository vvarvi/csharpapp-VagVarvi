using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Infrastructure.Auth
{
    public class AuthDelegatingHandler : DelegatingHandler
    {
        private readonly ITokenProvider _tokenProvider;

        public AuthDelegatingHandler(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenProvider.GetAccessTokenAsync();

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            Console.WriteLine("TOKEN ATTACHED", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
