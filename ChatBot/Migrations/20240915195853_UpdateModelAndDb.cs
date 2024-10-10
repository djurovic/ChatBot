using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatBot.Migrations
{
    public partial class UpdateModelAndDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MicroserviceMethods_MicroserviceCatalogs_MicroserviceCatalogId",
                table: "MicroserviceMethods");

            migrationBuilder.DropColumn(
                name: "MicroserviceName",
                table: "MicroserviceCatalogs");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionExample",
                table: "MicroserviceMethods",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "MicroserviceCatalogId",
                table: "MicroserviceMethods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MethodName",
                table: "MicroserviceMethods",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MethodLink",
                table: "MicroserviceMethods",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MainLink",
                table: "MicroserviceCatalogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MicroserviceCatalogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MicroserviceMethods_MicroserviceCatalogs_MicroserviceCatalogId",
                table: "MicroserviceMethods",
                column: "MicroserviceCatalogId",
                principalTable: "MicroserviceCatalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MicroserviceMethods_MicroserviceCatalogs_MicroserviceCatalogId",
                table: "MicroserviceMethods");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MicroserviceCatalogs");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionExample",
                table: "MicroserviceMethods",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<int>(
                name: "MicroserviceCatalogId",
                table: "MicroserviceMethods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "MethodName",
                table: "MicroserviceMethods",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "MethodLink",
                table: "MicroserviceMethods",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "MainLink",
                table: "MicroserviceCatalogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "MicroserviceName",
                table: "MicroserviceCatalogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MicroserviceMethods_MicroserviceCatalogs_MicroserviceCatalogId",
                table: "MicroserviceMethods",
                column: "MicroserviceCatalogId",
                principalTable: "MicroserviceCatalogs",
                principalColumn: "Id");
        }
    }
}
