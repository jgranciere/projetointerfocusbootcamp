using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AprendendoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNovaColunaProdutosMaisPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "categoria",
                schema: "public",
                table: "produtos_mais_pedidos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "categoria",
                schema: "public",
                table: "produtos_mais_pedidos");
        }
    }
}
