using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Czar.AbpDemo.Migrations
{
    public partial class CreateBookEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId1",
                table: "AbpEntityPropertyChanges");

            migrationBuilder.DropIndex(
                name: "IX_AbpEntityPropertyChanges_EntityChangeId1",
                table: "AbpEntityPropertyChanges");

            migrationBuilder.DropColumn(
                name: "EntityChangeId1",
                table: "AbpEntityPropertyChanges");

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityChangeId1",
                table: "AbpEntityPropertyChanges",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityPropertyChanges_EntityChangeId1",
                table: "AbpEntityPropertyChanges",
                column: "EntityChangeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId1",
                table: "AbpEntityPropertyChanges",
                column: "EntityChangeId1",
                principalTable: "AbpEntityChanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
