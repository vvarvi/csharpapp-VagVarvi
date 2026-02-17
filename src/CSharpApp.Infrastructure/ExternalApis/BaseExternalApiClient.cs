using CSharpApp.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Infrastructure.ExternalApis
{
    public abstract class BaseExternalApiClient : IExternalApiClient
    {
        public virtual string SourceName { get; }

        private readonly HttpClient _httpClient;

        public BaseExternalApiClient(HttpClient httpClient, string sourceName)
        {
            _httpClient = httpClient;
            SourceName = sourceName;
        }

        public Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
