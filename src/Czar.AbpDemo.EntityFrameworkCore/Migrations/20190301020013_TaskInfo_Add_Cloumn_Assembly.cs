using Microsoft.EntityFrameworkCore.Migrations;

namespace Czar.AbpDemo.Migrations
{
    public partial class TaskInfo_Add_Cloumn_Assembly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobAssemblyName",
                table: "TaskInfo",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobClassName",
                table: "TaskInfo",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobNamespace",
                table: "TaskInfo",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobAssemblyName",
                table: "TaskInfo");

            migrationBuilder.DropColumn(
                name: "JobClassName",
                table: "TaskInfo");

            migrationBuilder.DropColumn(
                name: "JobNamespace",
                table: "TaskInfo");
        }
    }
}
