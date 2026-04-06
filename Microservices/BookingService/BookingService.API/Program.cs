using System.Text;
using BookingService.API.Extensions;
using BookingService.API.Middlewares;
using BookingService.Application.IServices;
using BookingService.Application.Services;
using BookingService.Application.Settings;
using BookingService.Core.Interfaces;
using BookingService.Infrastructure.Data;
using BookingService.Infrastructure.HttpClients;
using BookingService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using Serilog;

namespace BookingService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var builder = WebApplication.CreateBuilder(args);

            // Serilog Settings
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();
            builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
            builder.Services.AddLogging();

            // Middlewares
            builder.Services.AddTransient<ExceptionHandlingMiddleware>();
            builder.Services.AddTransient<RequestResponseLoggingMiddleware>();

            // Repositories & UnitOfWork
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            builder.Services.AddScoped<IBookingService, BookingAppService>();

            // JWT Settings
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

            // HttpClient for RoomApiClient
            builder.Services.AddHttpClient<IRoomApiClient, RoomApiClient>(client =>
            {
                var roomServiceUrl = builder.Configuration["ServiceUrls:RoomService"]
                    ?? "http://room-service:8080";
                client.BaseAddress = new Uri(roomServiceUrl);
            });

            // DbContext
            builder.Services.AddDbContext<BookingDbContext>(option =>
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

            // Auto-apply migrations
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
                db.Database.Migrate();
            }

            app.UseCors("AllowReactApp");
            app.UseHttpMetrics();
            app.UseCustomMiddlewares();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapMetrics();

            app.Run();
        }
    }
}
