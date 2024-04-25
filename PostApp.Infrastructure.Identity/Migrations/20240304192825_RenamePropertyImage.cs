using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostApp.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class RenamePropertyImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                schema: "Identity",
                table: "Users",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                schema: "Identity",
                table: "Users",
                newName: "ImagePath");
        }
    }
}
