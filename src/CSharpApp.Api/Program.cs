using Asp.Versioning;
using CSharpApp.Api.Middleware;
using CSharpApp.Application;
using CSharpApp.Application.UseCases.Categories.Commands;
using CSharpApp.Application.UseCases.Categories.Queries;
using CSharpApp.Application.UseCases.Products.Commands;
using CSharpApp.Application.UseCases.Products.Queries;
using CSharpApp.Infrastructure.Auth;
using CSharpApp.Infrastructure.Health;
using CSharpApp.Infrastructure.Security.Jwt;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration(builder.Configuration);
builder.Services.AddHttpConfiguration(builder.Configuration);
//builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();
//builder.Services.AddApiVersioning();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IProductsService>());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

builder.Services.AddHealthChecks()
    .AddCheck<ExternalApiHealthCheck>(
        "external_api",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "ready" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<PerformanceLoggingMiddleware>();

var versionedEndpointRouteBuilder = app.NewVersionedApi();

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/token-test", async (IAuthService authService) =>
    {
        var token = await authService.LoginAsync();
        return token;
    })
.HasApiVersion(1.0);


versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetProductsQuery());

    if (!result.IsSuccess)
    {
        return Results.Problem(
            detail: result.Error!.Message,
            statusCode: result.Error.StatusCode,
            title: result.Error.Code);
    }

    return Results.Ok(result.Value);
})
.HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/products/{id}", async (int id, IMediator mediator) =>
    {
        return await mediator.Send(new GetProductByIdQuery(id));
    })
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/products", async (CreateProductCommand command, IMediator mediator) =>
    {
        return await mediator.Send(command);
    })
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories", async (IMediator mediator) =>
{
    return await mediator.Send(new GetCategoriesQuery());
})
.HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/categories/{id}", async (int id, IMediator mediator) =>
{
    return await mediator.Send(new GetCategoryByIdQuery(id));
})
.HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/categories", async (CreateCategoryCommand command, IMediator mediator) =>
{
    return await mediator.Send(command);
})
.HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapHealthChecks("api/v{version:apiVersion}/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                error = e.Value.Exception?.Message
            })
        });

        await context.Response.WriteAsync(result);
    }
});

//app.MapControllers();

app.Run();

public partial class Program { }