using CSharpApp.Core.Commands.DTO;
using CSharpApp.Core.Common;
using CSharpApp.Core.Entities;
using CSharpApp.Core.Exceptions;
using System.Net.Http.Json;

namespace CSharpApp.Application.UseCases.Products;

public class ProductsService : IProductsService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(HttpClient httpClient, ILogger<ProductsService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<Product>>> GetProducts(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync("products");

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync();

            throw new ExternalApiException((int)response.StatusCode, message);
        }

        var content = await response.Content.ReadAsStringAsync();

        var products = JsonSerializer.Deserialize<List<Product>>(
            content,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return Result<IReadOnlyCollection<Product>>.Success(
             products?.AsReadOnly() ?? new List<Product>().AsReadOnly());
        //products?.AsReadOnly() ?? new List<Product>().AsReadOnly();
    }

    public async Task<Product> GetProductById(int id)
    {
        var response = await _httpClient.GetAsync($"products/{id}");

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            throw new ExternalApiException((int)response.StatusCode, content);
        }

        return await response.Content
            .ReadFromJsonAsync<Product>()
            ?? throw new Exception("Product not found");
    }

    public async Task<Product> CreateProduct(CreateProductRequest request)
    {
        var response = await _httpClient.GetAsync("products");

        if (!response.IsSuccessStatusCode)
        {
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();

                throw new ExternalApiException((int)response.StatusCode, message);
            }
        }

        var contentString = await response.Content.ReadAsStringAsync();

        var product = JsonSerializer.Deserialize<Product>(contentString);

        return new Product
        {
            Id = product?.Id,
            Title = product.Title,
            Price = product.Price,
            Description = product.Description
        };
    }
}