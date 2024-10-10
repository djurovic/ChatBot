using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatBot.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MicroserviceCatalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MicroserviceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MicroserviceCatalogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MicroserviceMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MethodName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MethodLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionExample = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateInterpretationNeeded = table.Column<bool>(type: "bit", nullable: false),
                    MicroserviceCatalogId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MicroserviceMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MicroserviceMethods_MicroserviceCatalogs_MicroserviceCatalogId",
                        column: x => x.MicroserviceCatalogId,
                        principalTable: "MicroserviceCatalogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MicroserviceMethods_MicroserviceCatalogId",
                table: "MicroserviceMethods",
                column: "MicroserviceCatalogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MicroserviceMethods");

            migrationBuilder.DropTable(
                name: "MicroserviceCatalogs");
        }
    }
}
