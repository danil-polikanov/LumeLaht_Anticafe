using LumaCove_RoomApi;
using LumeLaht_RoomApi.Application.IServices;
using LumeLaht_RoomApi.Application.Mapping;
using LumeLaht_RoomApi.Application.Services;
using LumeLaht_RoomApi.Application.Settings;
using LumeLaht_RoomApi.Core_.Interfaces;
using LumeLaht_RoomApi.Extensions;
using LumeLaht_RoomApi.Infrastructure.Data;
using LumeLaht_RoomApi.Infrastructure.Repositories;
using LumeLaht_RoomApi.Middlewares;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Text.Json.Serialization;

namespace LumeLaht_RoomApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var builder = WebApplication.CreateBuilder(args);
            
            // Serilog Settings
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration) // settings from appsettings.json
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day) // new file every day
                .CreateLogger();

            // Include Serilog to ASP.NET
            builder.Host.UseSerilog();
            builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
            builder.Services.AddLogging();
            var mapperConfig = TypeAdapterConfig.GlobalSettings;
            mapperConfig.Scan(typeof(RoomProfile).Assembly);
            builder.Services.AddSingleton(mapperConfig);
            builder.Services.AddScoped<IMapper, ServiceMapper>();
            builder.Services.AddTransient<ExceptionHandlingMiddleware>();
            builder.Services.AddTransient<RequestResponseLoggingMiddleware>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IActivityService, ActivityService>();
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).LogTo(Console.WriteLine, LogLevel.Information);
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
            app.UseCors("AllowReactApp");
            app.UseCustomMiddlewares();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

    }
}
