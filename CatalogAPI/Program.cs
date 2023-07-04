using AutoMapper;
using CatalogAPI.Data;
using CatalogAPI.DTOs.Mappings;
using CatalogAPI.Extensions;
using CatalogAPI.Filters;
using CatalogAPI.Logging;
using CatalogAPI.Repositories;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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
            builder.Services.AddSwaggerGen(c =>
	        {
	    	    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CatalogAPI", Version = "v1"});
	
		        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
		        {
			        Name = "Authorization",
			        Type = SecuritySchemeType.ApiKey,
			        Scheme = "Bearer",
			        BearerFormat = "JWT",
			        In = ParameterLocation.Header,
			        Description = "Header de autorização JWT usando o esquema Bearer.\r\n\r\nInforme 'Bearer'[espaço] e o seu token.\r\n\r\nExemplo: \'Bearer 12345abcdef\'"
		         });

		        c.AddSecurityRequirement(new OpenApiSecurityRequirement
		        {
			        {
				        new OpenApiSecurityScheme
				        {
					        Reference = new OpenApiReference
					        {
						        Type = ReferenceType.SecurityScheme,
						        Id = "Bearer"
					        }				
				        },
                        new string[] {}
                    }
		         });
	        });

            string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");


            builder.Services.AddDbContext<CatalogAppDbContext>(options =>
                            options.UseMySql(mySqlConnection,
                            ServerVersion.AutoDetect(mySqlConnection)));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<CatalogAppDbContext>()
                            .AddDefaultTokenProviders();

	        /*Adiciona o manipulador de autenticacao e define o esquema de autenticacao
              usado (Bearer), valida o emissor, audiencia e a chave. Utilizando uma chave
              secreta validando a assinatura
	         */
            builder.Services.AddAuthentication(
		            JwtBearerDefaults.AuthenticationScheme)
		            .AddJwtBearer(options =>
		            options.TokenValidationParameters = new TokenValidationParameters
		            {
			            ValidateIssuer = true,
			            ValidateAudience = true,
			            ValidateLifetime = true,
			            ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
			            ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
			            ValidateIssuerSigningKey = true,
			            IssuerSigningKey = new SymmetricSecurityKey(Encoding
                                                                    .UTF8.GetBytes(builder.Configuration["Jwt:key"]))
		            });

            builder.Services.AddTransient<IMyService, MyService>();
            // builder.Services.AddScoped<APILoggingFilter>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
            {
                LogLevel = LogLevel.Information
            }));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);


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