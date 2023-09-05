using FluentValidation.AspNetCore;
using System.Reflection;
using HomeApi.Contracts;
using HomeApi.Contracts.Validation;
using HomeApi.Configuration;
using HomeApi.Data.Repos;
using HomeApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace HomeApi
{
    public class Program
    {
        /// <summary>
        /// Загрузка конфигурации из файла Json
        /// </summary>
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .AddJsonFile("HomeOptions.json")
            .Build();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);        

            // Добавляем новый сервис
            builder.Services.Configure<HomeOptions>(Configuration);

            // регистрация сервиса репозитория для взаимодействия с базой данных
            builder.Services.AddSingleton<IDeviceRepository, DeviceRepository>();
            builder.Services.AddSingleton<IRoomRepository, RoomRepository>();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<HomeApiContext>(options=>options.UseSqlServer(connectionString), ServiceLifetime.Singleton);

            // Подключаем валидацию
            builder.Services.AddFluentValidation(fv=>fv.RegisterValidatorsFromAssemblyContaining<AddDeviceRequestValidator>());

            // Загружаем только адресс (вложенный Json-объект))
            builder.Services.Configure<Address>(Configuration.GetSection("Address"));

            // Нам не нужны представления, но в MVC бы здесь стояло AddControllersWithViews()
            builder.Services.AddControllers();

            // поддерживает автоматическую генерацию документации WebApi с использованием Swagger
            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeApi", Version = "v1" }); });

            // Подключаем автомаппинг
            builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeApi v1"));

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}