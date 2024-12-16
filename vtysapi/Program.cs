using Domain.kisi;
using Domain.masa;
using Domain.musteri;
using Domain.personel;
using Domain.rezervasyon;
using Infrastructure.Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RestorantDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//AddScopedSection();

builder.Services.AddScoped<IRepository<Masa>, Repository<Masa>>();
builder.Services.AddScoped<IRepository<Musteri>, Repository<Musteri>>();
builder.Services.AddScoped<IRepository<Rezervasyon>, Repository<Rezervasyon>>();
builder.Services.AddScoped<IRepository<Kisi>, Repository<Kisi>>();
builder.Services.AddScoped<IRepository<Personel>, Repository<Personel>>();


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

