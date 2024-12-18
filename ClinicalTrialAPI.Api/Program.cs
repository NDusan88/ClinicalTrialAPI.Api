using ClinicalTrialAPI.Application.Interfaces;
using ClinicalTrialAPI.Application.Services;
using ClinicalTrialAPI.Application.UseCases;
using ClinicalTrialAPI.Domain.Repositories;
using ClinicalTrialAPI.Infrastructure.Persistence;
using ClinicalTrialAPI.Api.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<ClinicalTrialAPI.Application.Validators.ClinicalTrialValidator>()
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClinicalTrialService, ClinicalTrialService>();
builder.Services.AddScoped<IClinicalTrialRepository, ClinicalTrialRepository>();

builder.Services.AddScoped<AddClinicalTrialUseCase>();
builder.Services.AddScoped<GetClinicalTrialByIdUseCase>();
builder.Services.AddScoped<GetAllClinicalTrialsByStatusUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
