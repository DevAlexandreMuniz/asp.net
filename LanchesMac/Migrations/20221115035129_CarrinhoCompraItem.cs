using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanchesMac.Migrations
{
    public partial class CarrinhoCompraItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "quantidade",
                table: "CarrinhoCompraItens",
                newName: "Quantidade");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "CarrinhoCompraItens",
                newName: "quantidade");
        }
    }
}
