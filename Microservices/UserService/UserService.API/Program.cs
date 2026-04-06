using System.Text;
using UserService.Application.IServices;
using UserService.Application.Services;
using UserService.Application.Settings;
using UserService.Core.Interfaces;
using UserService.API.Extensions;
using UserService.API.Middlewares;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using Serilog;

namespace UserService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var builder = WebApplication.CreateBuilder(args);

            // Serilog Settings
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Include Serilog to ASP.NET
            builder.Host.UseSerilog();
            builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
            builder.Services.AddLogging();

            // Middlewares
            builder.Services.AddTransient<ExceptionHandlingMiddleware>();
            builder.Services.AddTransient<RequestResponseLoggingMiddleware>();

            // Repositories & UnitOfWork
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, Application.Services.UserService>();

            // Settings
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

            // DbContext
            builder.Services.AddDbContext<UserDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                      .LogTo(Console.WriteLine, LogLevel.Information);
            });

            // JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };
                });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Auto-apply migrations in Docker/Production
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
                db.Database.EnsureCreated();
            }

            app.UseCors("AllowReactApp");
            app.UseHttpMetrics();
            app.UseCustomMiddlewares();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapMetrics();

            app.Run();
        }
    }
}
