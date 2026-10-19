using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroLibro.Migrations
{
    /// <inheritdoc />
    public partial class detalleAhora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_estudianteDetalles_Estudiantes_ClienteId",
                table: "estudianteDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_estudianteDetalles_Libros_LibroId",
                table: "estudianteDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_estudianteDetalles_Prestamos_PrestamoId",
                table: "estudianteDetalles");

            migrationBuilder.DropIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_estudianteDetalles",
                table: "estudianteDetalles");

            migrationBuilder.DropIndex(
                name: "IX_estudianteDetalles_ClienteId",
                table: "estudianteDetalles");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "estudianteDetalles");

            migrationBuilder.RenameTable(
                name: "estudianteDetalles",
                newName: "EstudianteDetalle");

            migrationBuilder.RenameIndex(
                name: "IX_estudianteDetalles_PrestamoId",
                table: "EstudianteDetalle",
                newName: "IX_EstudianteDetalle_PrestamoId");

            migrationBuilder.RenameIndex(
                name: "IX_estudianteDetalles_LibroId",
                table: "EstudianteDetalle",
                newName: "IX_EstudianteDetalle_LibroId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstudianteDetalle",
                table: "EstudianteDetalle",
                column: "DetalleId");

            migrationBuilder.CreateTable(
                name: "Entradas",
                columns: table => new
                {
                    EntradaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entradas", x => x.EntradaId);
                });

            migrationBuilder.CreateTable(
                name: "PrestamoDetalles",
                columns: table => new
                {
                    PrestamoDetalleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrestamoId = table.Column<int>(type: "int", nullable: false),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestamoDetalles", x => x.PrestamoDetalleId);
                    table.ForeignKey(
                        name: "FK_PrestamoDetalles_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "LibroId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrestamoDetalles_Prestamos_PrestamoId",
                        column: x => x.PrestamoId,
                        principalTable: "Prestamos",
                        principalColumn: "PrestamoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntradasDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntradaId = table.Column<int>(type: "int", nullable: false),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradasDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntradasDetalle_Entradas_EntradaId",
                        column: x => x.EntradaId,
                        principalTable: "Entradas",
                        principalColumn: "EntradaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntradasDetalle_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "LibroId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradasDetalle_EntradaId",
                table: "EntradasDetalle",
                column: "EntradaId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradasDetalle_LibroId",
                table: "EntradasDetalle",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_PrestamoDetalles_LibroId",
                table: "PrestamoDetalles",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_PrestamoDetalles_PrestamoId",
                table: "PrestamoDetalles",
                column: "PrestamoId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstudianteDetalle_Libros_LibroId",
                table: "EstudianteDetalle",
                column: "LibroId",
                principalTable: "Libros",
                principalColumn: "LibroId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EstudianteDetalle_Prestamos_PrestamoId",
                table: "EstudianteDetalle",
                column: "PrestamoId",
                principalTable: "Prestamos",
                principalColumn: "PrestamoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstudianteDetalle_Libros_LibroId",
                table: "EstudianteDetalle");

            migrationBuilder.DropForeignKey(
                name: "FK_EstudianteDetalle_Prestamos_PrestamoId",
                table: "EstudianteDetalle");

            migrationBuilder.DropTable(
                name: "EntradasDetalle");

            migrationBuilder.DropTable(
                name: "PrestamoDetalles");

            migrationBuilder.DropTable(
                name: "Entradas");

            migrationBuilder.DropIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstudianteDetalle",
                table: "EstudianteDetalle");

            migrationBuilder.RenameTable(
                name: "EstudianteDetalle",
                newName: "estudianteDetalles");

            migrationBuilder.RenameIndex(
                name: "IX_EstudianteDetalle_PrestamoId",
                table: "estudianteDetalles",
                newName: "IX_estudianteDetalles_PrestamoId");

            migrationBuilder.RenameIndex(
                name: "IX_EstudianteDetalle_LibroId",
                table: "estudianteDetalles",
                newName: "IX_estudianteDetalles_LibroId");

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "estudianteDetalles",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_estudianteDetalles",
                table: "estudianteDetalles",
                column: "DetalleId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos",
                column: "LibroId",
                unique: true,
                filter: "[Estado] = N'Prestado'");

            migrationBuilder.CreateIndex(
                name: "IX_estudianteDetalles_ClienteId",
                table: "estudianteDetalles",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_estudianteDetalles_Estudiantes_ClienteId",
                table: "estudianteDetalles",
                column: "ClienteId",
                principalTable: "Estudiantes",
                principalColumn: "EstudianteId");

            migrationBuilder.AddForeignKey(
                name: "FK_estudianteDetalles_Libros_LibroId",
                table: "estudianteDetalles",
                column: "LibroId",
                principalTable: "Libros",
                principalColumn: "LibroId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_estudianteDetalles_Prestamos_PrestamoId",
                table: "estudianteDetalles",
                column: "PrestamoId",
                principalTable: "Prestamos",
                principalColumn: "PrestamoId");
        }
    }
}
