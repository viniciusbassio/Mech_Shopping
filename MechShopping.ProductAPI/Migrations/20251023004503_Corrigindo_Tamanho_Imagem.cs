using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MechShopping.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class Corrigindo_Tamanho_Imagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "imagem_url",
                table: "Produto",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Produto",
                columns: new[] { "id", "Nome_Categoria", "Descricao", "imagem_url", "Nome_Produto", "Preco" },
                values: new object[] { 7L, "Freios", "Cilindro de freio para carros de passeio", "https://mecshopping.vtexassets.com/arquivos/ids/155631-800-auto?v=637973292662870000&width=800&height=auto&aspect=true", "Cilindro de freio", 350m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Produto",
                keyColumn: "id",
                keyValue: 7L);

            migrationBuilder.AlterColumn<string>(
                name: "imagem_url",
                table: "Produto",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
