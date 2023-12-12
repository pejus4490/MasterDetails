using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterDetails.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePurchaseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Purchases");
        }
    }
}
