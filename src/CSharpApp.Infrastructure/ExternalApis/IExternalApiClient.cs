using CSharpApp.Core.Dtos;
using CSharpApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Infrastructure.ExternalApis
{
    public interface IExternalApiClient
    {
        string SourceName { get; }

        Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken);
    }
}
