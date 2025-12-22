using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BepopJWT.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Artists_ArtistId",
                table: "Songs");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Categories_CategoryId",
                table: "Songs");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Packages_PackageId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_PackageId",
                table: "Songs");

            migrationBuilder.RenameColumn(
                name: "PackageId",
                table: "Songs",
                newName: "MinLevelRequired");

            migrationBuilder.RenameColumn(
                name: "RequiredLevel",
                table: "Packages",
                newName: "PackageLevel");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Songs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ArtistId",
                table: "Songs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Artists_ArtistId",
                table: "Songs",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ArtistId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Categories_CategoryId",
                table: "Songs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Artists_ArtistId",
                table: "Songs");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Categories_CategoryId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "MinLevelRequired",
                table: "Songs",
                newName: "PackageId");

            migrationBuilder.RenameColumn(
                name: "PackageLevel",
                table: "Packages",
                newName: "RequiredLevel");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Songs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ArtistId",
                table: "Songs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_PackageId",
                table: "Songs",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Artists_ArtistId",
                table: "Songs",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Categories_CategoryId",
                table: "Songs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Packages_PackageId",
                table: "Songs",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "PackageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
