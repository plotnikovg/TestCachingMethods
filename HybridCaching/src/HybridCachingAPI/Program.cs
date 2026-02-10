using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using HybridCachingDomain.Interfaces;
using HybridCachingInfrastructure;
using HybridCachingInfrastructure.Repositories;
using HybridCachingApplication.Products.Queries;
using HybridCachingInfrastructure.Repositories.Cached;
using Microsoft.Extensions.Caching.Hybrid;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetProductQuery).Assembly);
});

builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromSeconds(60),
        LocalCacheExpiration = TimeSpan.FromSeconds(12)
    };
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
});

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<IProductRepository>(provider =>
{
    var repository = provider.GetRequiredService<ProductRepository>();
    var cache = provider.GetRequiredService<HybridCache>();

    return new CachedProductRepository(repository, cache);
});

builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ICategoryRepository>(provider =>
{
    var repository = provider.GetRequiredService<CategoryRepository>();
    var cache = provider.GetRequiredService<HybridCache>();

    return new CachedCategoryRepository(repository, cache);
});

var connectionString =
    builder.Configuration.GetConnectionString("PostgresConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<AppDbContextInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();
    await initializer.InitializeAsync();
    await initializer.SeedAsync();
}

app.Run();