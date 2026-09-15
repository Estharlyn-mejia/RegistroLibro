using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroLibro.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAutor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Autor",
                table: "Libros",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Autor",
                table: "Libros");
        }
    }
}
