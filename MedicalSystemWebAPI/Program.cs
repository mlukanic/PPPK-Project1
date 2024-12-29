using MedicalSystemClassLibrary;
using MedicalSystemClassLibrary.Repositories;
using MedicalSystemClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MedicalSystemClassLibrary.Factories;
using MedicalSystemClassLibrary.Data;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<MedicalSystemDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgre")));

// Register repositories
builder.Services.AddScoped<IRepository<Patient>, PatientRepository>();

// Register the factory
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();

// Register services
builder.Services.AddScoped<PatientService>();


// Add controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MedicalSystem API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MedicalSystem API v1"));
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
