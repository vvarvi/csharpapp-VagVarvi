using CSharpApp.Application.Abstractions;
using CSharpApp.Core.Commands.DTO;
using CSharpApp.Core.Entities;
using CSharpApp.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

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

        public Task<Category> CreateCategory(CreateCategoryRequest request)
        {
            throw new NotImplementedException();
        }
    }

}
