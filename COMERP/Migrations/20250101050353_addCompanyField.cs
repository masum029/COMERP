using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COMERP.Migrations
{
    /// <inheritdoc />
    public partial class addCompanyField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Companys",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Companys");
        }
    }
}
