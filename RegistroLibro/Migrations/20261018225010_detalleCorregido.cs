using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroLibro.Migrations
{
    /// <inheritdoc />
    public partial class detalleCorregido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "estudianteDetalles",
                columns: table => new
                {
                    DetalleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrestamoId = table.Column<int>(type: "int", nullable: false),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estudianteDetalles", x => x.DetalleId);
                    table.ForeignKey(
                        name: "FK_estudianteDetalles_Estudiantes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Estudiantes",
                        principalColumn: "EstudianteId");
                    table.ForeignKey(
                        name: "FK_estudianteDetalles_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "LibroId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_estudianteDetalles_Prestamos_PrestamoId",
                        column: x => x.PrestamoId,
                        principalTable: "Prestamos",
                        principalColumn: "PrestamoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_estudianteDetalles_ClienteId",
                table: "estudianteDetalles",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_estudianteDetalles_LibroId",
                table: "estudianteDetalles",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_estudianteDetalles_PrestamoId",
                table: "estudianteDetalles",
                column: "PrestamoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "estudianteDetalles");
        }
    }
}
