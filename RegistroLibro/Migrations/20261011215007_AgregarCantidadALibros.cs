using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroLibro.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCantidadALibros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "Libros",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_EstudianteId",
                table: "Prestamos",
                column: "EstudianteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Estudiantes_EstudianteId",
                table: "Prestamos",
                column: "EstudianteId",
                principalTable: "Estudiantes",
                principalColumn: "EstudianteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Libros_LibroId",
                table: "Prestamos",
                column: "LibroId",
                principalTable: "Libros",
                principalColumn: "LibroId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Estudiantes_EstudianteId",
                table: "Prestamos");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Libros_LibroId",
                table: "Prestamos");

            migrationBuilder.DropIndex(
                name: "IX_Prestamos_EstudianteId",
                table: "Prestamos");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "Libros");
        }
    }
}
