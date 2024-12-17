using Domain.gider;
using Domain.kategori;
using Domain.kisi;
using Domain.malzeme;
using Domain.masa;
using Domain.menu;
using Domain.musteri;
using Domain.odeme;
using Domain.odemeturu;
using Domain.personel;
using Domain.rezervasyon;
using Domain.siparis;
using Domain.siparisdetay;
using Domain.stok;
using Domain.tedarikci;
using Domain.tedariksiparisi;
using Infrastructure.Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RestorantDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run("https://*:5003"); // 👈 Use the provided URL when running the application

Environment.SetEnvironmentVariable("API_BASE_URL", app.Urls.First());


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Masa>, Repository<Masa>>();
        services.AddScoped<IRepository<Musteri>, Repository<Musteri>>();
        services.AddScoped<IRepository<Rezervasyon>, Repository<Rezervasyon>>();
        services.AddScoped<IRepository<Kisi>, Repository<Kisi>>();
        services.AddScoped<IRepository<Personel>, Repository<Personel>>();
        services.AddScoped<IRepository<Gider>, Repository<Gider>>();
        services.AddScoped<IRepository<Kategori>, Repository<Kategori>>();
        services.AddScoped<IRepository<Malzeme>, Repository<Malzeme>>();
        services.AddScoped<IRepository<Menu>, Repository<Menu>>();
        services.AddScoped<IRepository<Odeme>, Repository<Odeme>>();
        services.AddScoped<IRepository<OdemeTuru>, Repository<OdemeTuru>>();
        services.AddScoped<IRepository<Siparis>, Repository<Siparis>>();
        services.AddScoped<IRepository<SiparisDetay>, Repository<SiparisDetay>>();
        services.AddScoped<IRepository<Stok>, Repository<Stok>>();
        services.AddScoped<IRepository<Tedarikci>, Repository<Tedarikci>>();
        services.AddScoped<IRepository<TedarikSiparisi>, Repository<TedarikSiparisi>>();

        return services; 
    }
}
