using Microsoft.EntityFrameworkCore;
using RegistroPrestamos.Data;
using RegistroPrestamos.Services;
using Scalar.AspNetCore;
using RegistroEstudiante.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var ConStr = builder.Configuration.GetConnectionString("SqlConStr");
builder.Services.AddDbContextFactory<RegistrolibrodbContext>(op => op.UseSqlServer(ConStr));

builder.Services.AddControllers();

builder.Services.AddScoped<LibrosServices>();
builder.Services.AddScoped<EstudiantesServices>();
builder.Services.AddScoped<PrestamosServices>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();