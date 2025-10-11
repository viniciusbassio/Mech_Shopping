using Microsoft.EntityFrameworkCore;

namespace MechShopping.ProductAPI.Model.Context
{
    public class SQLServerContext : DbContext
    {
        public SQLServerContext()   {}
        public SQLServerContext(DbContextOptions<SQLServerContext> options) : base(options) {}

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(new Product{
                Id = 7,
                Name="Cilindro de freio",
                Price= new decimal(350.00),
                Description="Cilindro de freio para carros de passeio",
                CategoryName="Freios",
                ImageURL= "https://mecshopping.vtexassets.com/arquivos/ids/155631-800-auto?v=637973292662870000&width=800&height=auto&aspect=true",
            });
        }
    }
}
