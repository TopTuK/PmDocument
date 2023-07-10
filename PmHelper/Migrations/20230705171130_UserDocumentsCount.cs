using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PmHelper.Migrations
{
    /// <inheritdoc />
    public partial class UserDocumentsCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentsCount",
                table: "UserProfile",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentsCount",
                table: "UserProfile");
        }
    }
}
