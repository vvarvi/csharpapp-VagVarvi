using CSharpApp.Application.Abstractions;
using CSharpApp.Application.UseCases.Products.Queries;
using CSharpApp.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpApp.Application.UseCases.Categories.Queries
{
    public sealed class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyCollection<Category>>
    {
        private readonly ICategoriesService _categoriesService;

        public GetCategoriesHandler(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<IReadOnlyCollection<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoriesService.GetCategories(cancellationToken);
        }
    }
}
