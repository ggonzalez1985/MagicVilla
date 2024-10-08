using MagicVilla_API;
using MagicVilla_API.Datos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MagicVilla_API.Repositorio.IRepositorio;
using MagicVilla_API.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//agrega el servicio con la cadena que 'option' y dentro de este toma la cadena DefaultConnection
builder.Services.AddDbContext<AplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(typeof(MappingConfig));

//para inyectar el servicio en cualquier lugar.
builder.Services.AddScoped<IVillaRepositorio, VillaRepositorio>();
builder.Services.AddScoped<INumeroVillaRepositorio, NumeroVillaRepositorio>();

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

app.Run();
