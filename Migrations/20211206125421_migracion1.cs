using Microsoft.EntityFrameworkCore.Migrations;

namespace bancontinental.Migrations
{
    public partial class migracion1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bancos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    banco = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bancos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bancosCuentas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idBanco = table.Column<int>(type: "int", nullable: false),
                    nroCuenta = table.Column<int>(type: "int", nullable: false),
                    saldo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bancosCuentas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bancosCuentasTransacciones",
                columns: table => new
                {
                    idNroTransaccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nroCuentaOrigen = table.Column<int>(type: "int", nullable: false),
                    idBancoOrigen = table.Column<int>(type: "int", nullable: false),
                    nroCuentaDestino = table.Column<int>(type: "int", nullable: false),
                    idBancoDestino = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    envio = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bancosCuentasTransacciones", x => x.idNroTransaccion);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bancos");

            migrationBuilder.DropTable(
                name: "bancosCuentas");

            migrationBuilder.DropTable(
                name: "bancosCuentasTransacciones");
        }
    }
}
