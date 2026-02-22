using CSharpApp.Application.Abstractions;
using CSharpApp.Core.Commands.DTO;
using CSharpApp.Core.Entities;
using CSharpApp.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CSharpApp.Infrastructure.Services
{
    public sealed class CategoriesService : ICategoriesService
    {
        private readonly HttpClient _httpClient;

        public CategoriesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<Category>> GetCategories(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("categories");

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                throw new ExternalApiException((int)response.StatusCode, content);
            }

            var categories = await response.Content.ReadFromJsonAsync<List<Category>>();

            return categories?.AsReadOnly() ?? new List<Category>().AsReadOnly();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var response = await _httpClient.GetAsync($"categories/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                throw new ExternalApiException((int)response.StatusCode, content);
            }

            return await response.Content.ReadFromJsonAsync<Category>() ?? throw new Exception("Category not found");
        }

        public async Task<Category> CreateCategory(CreateCategoryRequest request)
        {
            var response = await _httpClient.GetAsync("categories");

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();

                throw new ExternalApiException((int)response.StatusCode, message);
            }

            var contentString = await response.Content.ReadAsStringAsync();

            var product = JsonSerializer.Deserialize<Category>(contentString);

            return new Category
            {
                Name = request.Name,
                Image = request.Image
            };
        }
    }

}
