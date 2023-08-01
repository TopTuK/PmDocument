using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PmHelper.Migrations
{
    /// <inheritdoc />
    public partial class UserDocumentRequestText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestText",
                table: "UserDocument",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestText",
                table: "UserDocument");
        }
    }
}
