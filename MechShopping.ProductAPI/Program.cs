using AutoMapper;
using MechShopping.ProductAPI.Config;
using MechShopping.ProductAPI.Model.Context;
using MechShopping.ProductAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace MechShopping.ProductAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connection = builder.Configuration.GetConnectionString("SqlServerConnection");

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Repositórios
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            // DbContext com pooling
            builder.Services.AddDbContextPool<SQLServerContext>(options =>
                options.UseSqlServer(connection));

            // Controllers
            builder.Services.AddControllers();

            // Authentication - JWT Bearer
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:4435/";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                        //ValidateAudience = true,
                        //ValidAudience = "mech_shopping"
                    };
                });

            // Authorization policy
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "mech_shopping");
                });
            });

            // Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MechShopping.ProductAPI", Version = "v1" });
                c.EnableAnnotations();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Enter 'Bearer' [space] and your token.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
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
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            var app = builder.Build();

            // Swagger antes de tudo
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductAPI v1");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
