using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AprendendoAPI.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirRelacionamentoMaisPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_produtos_mais_pedidos_produtos_produto_id",
                schema: "public",
                table: "produtos_mais_pedidos");

            migrationBuilder.RenameTable(
                name: "produtos_mais_pedidos",
                schema: "public",
                newName: "produtos_mais_pedidos");

            migrationBuilder.RenameTable(
                name: "produtos",
                schema: "public",
                newName: "produtos");

            migrationBuilder.AddColumn<int>(
                name: "ProdutoId1",
                table: "produtos_mais_pedidos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_produtos_mais_pedidos_ProdutoId1",
                table: "produtos_mais_pedidos",
                column: "ProdutoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_produtos_mais_pedidos_produtos_ProdutoId1",
                table: "produtos_mais_pedidos",
                column: "ProdutoId1",
                principalTable: "produtos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_produtos_mais_pedidos_produtos_produto_id",
                table: "produtos_mais_pedidos",
                column: "produto_id",
                principalTable: "produtos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_produtos_mais_pedidos_produtos_ProdutoId1",
                table: "produtos_mais_pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_produtos_mais_pedidos_produtos_produto_id",
                table: "produtos_mais_pedidos");

            migrationBuilder.DropIndex(
                name: "IX_produtos_mais_pedidos_ProdutoId1",
                table: "produtos_mais_pedidos");

            migrationBuilder.DropColumn(
                name: "ProdutoId1",
                table: "produtos_mais_pedidos");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "produtos_mais_pedidos",
                newName: "produtos_mais_pedidos",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "produtos",
                newName: "produtos",
                newSchema: "public");

            migrationBuilder.AddForeignKey(
                name: "FK_produtos_mais_pedidos_produtos_produto_id",
                schema: "public",
                table: "produtos_mais_pedidos",
                column: "produto_id",
                principalSchema: "public",
                principalTable: "produtos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
