using Domain.DTOs.EmailDTOs;
using Infrastructure.Data;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApi.ExtensionMethods.AuthConfig;
using WebApi.ExtensionMethods.RegisterService;
using WebApi.ExtensionMethods.SwaggerConfig;

var builder = WebApplication.CreateBuilder(args);

//serilog configuration 

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig!);

// connection to database && dependency injection
builder.Services.AddRegisterService(builder.Configuration);


// register swagger configuration
builder.Services.SwaggerService();


// authentications service
builder.Services.AddAuthConfigureService(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


// update database
try
{
    var serviceProvider = app.Services.CreateScope().ServiceProvider;
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();

    //seed data
    var seeder = serviceProvider.GetRequiredService<Seeder>();
    await seeder.SeedUser();
    await seeder.SeedRoles();
    await seeder.SeedUserRole();
}
catch (Exception)
{
    // ignored
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();