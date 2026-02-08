using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Repositories;
using ProductApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Dependency Injection - Repository ve Service Kayıtları
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Geliştirme Ortamında Swagger'ı Aktif Et
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1");
        options.RoutePrefix = string.Empty;
    });
}

// 6. HTTPS Yönlendirmesi
app.UseHttpsRedirection();

// 7. Authorization (şu an yok, ileride eklenebilir)
app.UseAuthorization();

// 8. Controller'ları Map Et
app.MapControllers();

// 9. Uygulamayı Başlat
app.Run();
