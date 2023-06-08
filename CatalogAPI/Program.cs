using CatalogAPI.Data;
using CatalogAPI.Extensions;
using CatalogAPI.Filters;
using CatalogAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CatalogAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");


            builder.Services.AddDbContext<CatalogAppDbContext>(options =>
                            options.UseMySql(mySqlConnection,
                            ServerVersion.AutoDetect(mySqlConnection)));

            builder.Services.AddTransient<IMyService, MyService>();
            builder.Services.AddScoped<APILoggingFilter>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Adiciona middleware de tratamento de erros
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}