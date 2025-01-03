using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COMERP.Migrations
{
    /// <inheritdoc />
    public partial class AddClientIconAndIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Clients");
        }
    }
}
