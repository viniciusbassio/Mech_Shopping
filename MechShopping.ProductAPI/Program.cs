
using MechShopping.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Smo;
namespace MechShopping.ProductAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connection = builder.Configuration.GetConnectionString("SqlServerConnection");

            builder.Services.AddDbContext<SQLServerContext>(options => options.UseSqlServer(connection));
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
