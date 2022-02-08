using API.Helpers;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
{
    // Add services to the container.

    services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddLogging(loggingBuilder =>
            {
                var config = configuration.GetSection("Logging");
                loggingBuilder.AddConfiguration(config);
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
//                 loggingBuilder.AddAzureWebAppDiagnostics();
// #if DEBUG
//                 loggingBuilder.AddFile(config);
// #endif
            });
            services.AddAutoMapper(typeof(MappingProfiles));
    services.AddDbContext<StoreContext>(x => x.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
    services.AddScoped<IProductRepository, ProductRepository>();
    services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<StoreContext>();
    await dataContext.Database.MigrateAsync();
    await StoredContextSeed.SeedAsync(dataContext);
}

{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseAuthorization();

    app.MapControllers();
}

app.Run();
