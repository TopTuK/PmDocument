using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PmHelper.Migrations
{
    /// <inheritdoc />
    public partial class FrontendDocumentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "DocumentType");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "DocumentType");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "DocumentType",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DocumentType",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DocumentType",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "DocumentType",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
