using API_Technical_Test;
using API_Technical_Test.Datos;
using API_Technical_Test.Modelos;
using API_Technical_Test.Repositorio;
using API_Technical_Test.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var myCorsPolicy = "_myCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myCorsPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Agregar Servicio para la Conexión a la Base de Datos
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseMySQL( // Usar UseMySQL en lugar de UseMySql
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

//Agregar Servico AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

//Creamos el servicio para la Base de Datos
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();

var app = builder.Build();
app.UseCors(myCorsPolicy);
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
