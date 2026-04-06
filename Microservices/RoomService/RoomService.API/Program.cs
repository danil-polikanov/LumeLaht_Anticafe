using RoomService.Application.IServices;
using RoomService.Application.Mapping;
using RoomService.Application.Services;
using RoomService.Application.Settings;
using RoomService.Core.Interfaces;
using RoomService.API.Extensions;
using RoomService.Infrastructure.Data;
using RoomService.Infrastructure.Repositories;
using RoomService.API.Middlewares;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;

namespace RoomService.API
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

            // Mapster configuration
            var mapperConfig = TypeAdapterConfig.GlobalSettings;
            mapperConfig.Scan(typeof(RoomProfile).Assembly);
            builder.Services.AddSingleton(mapperConfig);
            builder.Services.AddScoped<IMapper, ServiceMapper>();

            // Middlewares
            builder.Services.AddTransient<ExceptionHandlingMiddleware>();
            builder.Services.AddTransient<RequestResponseLoggingMiddleware>();

            // Repositories & UnitOfWork
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            builder.Services.AddScoped<IRoomService, RoomAppService>();
            builder.Services.AddScoped<IActivityService, ActivityService>();

            // Cloudinary
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
            builder.Services.AddScoped<IImageService, ImageService>();

            // DbContext
            builder.Services.AddDbContext<RoomDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .LogTo(Console.WriteLine, LogLevel.Information);
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

            // Auto-apply migrations on startup
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<RoomDbContext>();
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

            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthorization();

            app.MapControllers();
            app.MapMetrics();

            app.Run();
        }
    }
}
