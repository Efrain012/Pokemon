using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comprehension.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAutenticacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioID",
                table: "Reminder",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioID",
                table: "Note",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioID",
                table: "Event",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioID = table.Column<Guid>(type: "TEXT", nullable: false),
                    NombreUsuario = table.Column<string>(type: "TEXT", nullable: false),
                    HashContrasena = table.Column<string>(type: "TEXT", nullable: false),
                    Sal = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioID);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    PermisoID = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecursoID = table.Column<Guid>(type: "TEXT", nullable: false),
                    TipoRecurso = table.Column<string>(type: "TEXT", nullable: false),
                    UsuarioID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Rol = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.PermisoID);
                    table.ForeignKey(
                        name: "FK_Permisos_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    TokenID = table.Column<string>(type: "TEXT", nullable: false),
                    UsuarioID = table.Column<Guid>(type: "TEXT", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.TokenID);
                    table.ForeignKey(
                        name: "FK_Tokens_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_UsuarioID",
                table: "Reminder",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Note_UsuarioID",
                table: "Note",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_UsuarioID",
                table: "Event",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_UsuarioID",
                table: "Permisos",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UsuarioID",
                table: "Tokens",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_NombreUsuario",
                table: "Usuarios",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Usuarios_UsuarioID",
                table: "Event",
                column: "UsuarioID",
                principalTable: "Usuarios",
                principalColumn: "UsuarioID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Usuarios_UsuarioID",
                table: "Note",
                column: "UsuarioID",
                principalTable: "Usuarios",
                principalColumn: "UsuarioID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reminder_Usuarios_UsuarioID",
                table: "Reminder",
                column: "UsuarioID",
                principalTable: "Usuarios",
                principalColumn: "UsuarioID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Usuarios_UsuarioID",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Note_Usuarios_UsuarioID",
                table: "Note");

            migrationBuilder.DropForeignKey(
                name: "FK_Reminder_Usuarios_UsuarioID",
                table: "Reminder");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Reminder_UsuarioID",
                table: "Reminder");

            migrationBuilder.DropIndex(
                name: "IX_Note_UsuarioID",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Event_UsuarioID",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "UsuarioID",
                table: "Reminder");

            migrationBuilder.DropColumn(
                name: "UsuarioID",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "UsuarioID",
                table: "Event");
        }
    }
}
