using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COMERP.Migrations
{
    /// <inheritdoc />
    public partial class AddAboutEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abouts_Companys_CompanyId",
                table: "Abouts");

            migrationBuilder.DropForeignKey(
                name: "FK_AboutTopics_SubAbouts_SubAboutId",
                table: "AboutTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_SubAbouts_Abouts_AboutId",
                table: "SubAbouts");

            migrationBuilder.AlterColumn<string>(
                name: "AboutId",
                table: "SubAbouts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubAboutId",
                table: "AboutTopics",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Abouts_Companys_CompanyId",
                table: "Abouts",
                column: "CompanyId",
                principalTable: "Companys",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AboutTopics_SubAbouts_SubAboutId",
                table: "AboutTopics",
                column: "SubAboutId",
                principalTable: "SubAbouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubAbouts_Abouts_AboutId",
                table: "SubAbouts",
                column: "AboutId",
                principalTable: "Abouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abouts_Companys_CompanyId",
                table: "Abouts");

            migrationBuilder.DropForeignKey(
                name: "FK_AboutTopics_SubAbouts_SubAboutId",
                table: "AboutTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_SubAbouts_Abouts_AboutId",
                table: "SubAbouts");

            migrationBuilder.AlterColumn<string>(
                name: "AboutId",
                table: "SubAbouts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SubAboutId",
                table: "AboutTopics",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Abouts_Companys_CompanyId",
                table: "Abouts",
                column: "CompanyId",
                principalTable: "Companys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AboutTopics_SubAbouts_SubAboutId",
                table: "AboutTopics",
                column: "SubAboutId",
                principalTable: "SubAbouts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubAbouts_Abouts_AboutId",
                table: "SubAbouts",
                column: "AboutId",
                principalTable: "Abouts",
                principalColumn: "Id");
        }
    }
}
