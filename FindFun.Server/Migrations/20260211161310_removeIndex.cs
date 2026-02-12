using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindFun.Server.Migrations
{
    /// <inheritdoc />
    public partial class removeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "age_recommendation",
                table: "parks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: "all");

            migrationBuilder.AddColumn<decimal>(
                name: "entrance_fee",
                table: "parks",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "is_free",
                table: "parks",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "organizer",
                table: "parks",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "park_type",
                table: "parks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "number",
                table: "addresses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "age_recommendation",
                table: "parks");

            migrationBuilder.DropColumn(
                name: "entrance_fee",
                table: "parks");

            migrationBuilder.DropColumn(
                name: "is_free",
                table: "parks");

            migrationBuilder.DropColumn(
                name: "organizer",
                table: "parks");

            migrationBuilder.DropColumn(
                name: "park_type",
                table: "parks");

            migrationBuilder.DropColumn(
                name: "number",
                table: "addresses");
        }
    }
}
