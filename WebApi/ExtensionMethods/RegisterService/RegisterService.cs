using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Seed;
using Infrastructure.Services.AuthService;
using Infrastructure.Services.EmailService;
using Infrastructure.Services.FileService;
using Infrastructure.Services.HashService;
using Infrastructure.Services.ProductService;
using Infrastructure.Services.RoleService;
using Infrastructure.Services.UserRoleService;
using Infrastructure.Services.UserService;
using Microsoft.EntityFrameworkCore;

namespace WebApi.ExtensionMethods.RegisterService;

public static class RegisterService
{
    public static void AddRegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(configure =>
            configure.UseNpgsql(configuration.GetConnectionString("Connection")));

        services.AddScoped<Seeder>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IProductService,ProductService>();
        services.AddScoped<IRoleService,RoleService>();
        services.AddScoped<IUserRoleService,UserRoleService>();
        
    }
}