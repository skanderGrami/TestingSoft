global using TestingSoft_Backend.Models;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using TestingSoft_Backend.Utils;
global using TestingSoft_Backend.Repositories.UserRepo;
global using TestingSoft_Backend.Repositories.BuildRepo;
global using TestingSoft_Backend.Repositories.RoleRepo;
global using TestingSoft_Backend.Repositories.ScenarioRepo;
global using TestingSoft_Backend.Repositories.TestCaseRepo;
global using TestingSoft_Backend.Repositories.TestSuiteRepo;
global using TestingSoft_Backend.Repositories;
global using TestingSoft_Backend.Repositories.TestTypeRepo;
global using TestingSoft_Backend.Repositories.TypeValueRepo;
global using TestingSoft_Backend.Dtos;
global using Microsoft.AspNetCore.Cors.Infrastructure;
global using Microsoft.CodeAnalysis.Options;
global using Microsoft.Extensions.Options;
using TechTalk.SpecFlow.Assist;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<TestingSoftContext>(options =>
         options.UseSqlServer(builder.Configuration.GetConnectionString("dbcnx")));
builder.Services.AddMemoryCache();

// Ajoutez votre configuration de services de dépendances ici
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Assurez-vous d'ajuster le nom de l'implémentation si nécessaire
builder.Services.AddScoped<ITypeValueRepository, TypeValueRepository>();
builder.Services.AddScoped<IScenarioRepository, ScenarioRepository>();
//builder.Services.AddTransient<IScenarioRepository, ScenarioRepository>();
builder.Services.AddScoped<IBuildRepository, BuildRepository>();
builder.Services.AddScoped<ITestCaseRepository, TestCaseRepository>();
builder.Services.AddScoped<ITestSuiteRepository, TestSuiteRepository>();
builder.Services.AddScoped<ITestTypeRepository, TestTypeRepository>();
//builder.Services.AddScoped<IRoleRepository, RoleRepository>();
//builder.Services.AddScoped<ITestReportRepository, TestReportRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<IWebDriver, ChromeDriver>();
builder.Services.AddHttpContextAccessor();



builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicyBuilder", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
        // Vous pouvez configurer plus de règles ici si nécessaire
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

///////////////////////
///Configurer l'injection de dépendances
/*builder.Services.AddSingleton<IWebDriver>(sp =>
 {
     var options = new ChromeOptions();
     options.AddArgument("--headless");
     options.AddArgument("--no-sandbox");
     options.AddArgument("--disable-dev-shm-usage");
     return new ChromeDriver(options);
 });
builder.Services.AddControllersWithViews();*/
/////////////////////////

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"ReportsVideo")),
    RequestPath = new PathString("/ReportsVideo")
});
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors("CorsPolicyBuilder");

app.UseAuthorization();

app.MapControllers();

// Obtenir le chemin du répertoire de travail actuel
//string pathProject = System.Environment.CurrentDirectory;


app.Run();
