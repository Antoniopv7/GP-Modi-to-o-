using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestion_Del_Presupuesto.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConvenioModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CentrosDeSaludId",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "Id_Retribucion",
                table: "Convenios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CentrosDeSaludId",
                table: "Convenios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_Retribucion",
                table: "Convenios",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
