using Business;
using Data;
using Entity.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("name=DefaultConn")
);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


/// Definicion de Servicios 
builder.Services.AddScoped<RolBusiness>();
builder.Services.AddScoped<RolData>();

builder.Services.AddScoped<PermissionBusiness>();
builder.Services.AddScoped<PermissionData>();

builder.Services.AddScoped<ModuleBusiness>();
builder.Services.AddScoped<ModuleData>();

builder.Services.AddScoped<UserBusiness>();
builder.Services.AddScoped<UserData>();

builder.Services.AddScoped<FormBusiness>();
builder.Services.AddScoped<FormData>();

builder.Services.AddScoped<ModuleFormBusiness>();
builder.Services.AddScoped<ModuleFormData>();

var OrigenesPermitidos = builder.Configuration.GetValue<String>("OrigenesPermitidos")!.Split(",");

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
