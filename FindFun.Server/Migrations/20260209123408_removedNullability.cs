using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindFun.Server.Migrations
{
    /// <inheritdoc />
    public partial class removedNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_addresses_streets_street_id",
                table: "addresses");

            migrationBuilder.AlterColumn<int>(
                name: "street_id",
                table: "addresses",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_addresses_streets_street_id",
                table: "addresses",
                column: "street_id",
                principalTable: "streets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_addresses_streets_street_id",
                table: "addresses");

            migrationBuilder.AlterColumn<int>(
                name: "street_id",
                table: "addresses",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_addresses_streets_street_id",
                table: "addresses",
                column: "street_id",
                principalTable: "streets",
                principalColumn: "id");
        }
    }
}
