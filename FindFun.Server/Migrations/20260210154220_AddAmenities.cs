using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FindFun.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddAmenities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "amenities",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_amenities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "park_amenities",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    park_id = table.Column<int>(type: "integer", nullable: false),
                    amenity_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_park_amenities", x => x.id);
                    table.ForeignKey(
                        name: "FK_park_amenities_amenities_amenity_id",
                        column: x => x.amenity_id,
                        principalTable: "amenities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_park_amenities_parks_park_id",
                        column: x => x.park_id,
                        principalTable: "parks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_park_amenities_amenity_id",
                table: "park_amenities",
                column: "amenity_id");

            migrationBuilder.CreateIndex(
                name: "IX_park_amenities_park_id",
                table: "park_amenities",
                column: "park_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "park_amenities");

            migrationBuilder.DropTable(
                name: "amenities");
        }
    }
}
