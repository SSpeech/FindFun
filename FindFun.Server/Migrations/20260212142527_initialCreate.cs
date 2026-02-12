using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FindFun.Server.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

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
                name: "georef-spain-municipio-millesime",
                schema: "public",
                columns: table => new
                {
                    gid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    year = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    official_co = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    official_na = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    official_co__3 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    official_na_4 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    official_co__5 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    official_na__6 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    iso_3166_3 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    local_name_ = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    geom = table.Column<Geometry>(type: "geometry(MultiPolygon,4326)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_georef-spain-municipio-millesime", x => x.gid);
                });

            migrationBuilder.CreateTable(
                name: "streets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    municipio_gid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streets", x => x.id);
                    table.ForeignKey(
                        name: "FK_streets_georef-spain-municipio-millesime_municipio_gid",
                        column: x => x.municipio_gid,
                        principalSchema: "public",
                        principalTable: "georef-spain-municipio-millesime",
                        principalColumn: "gid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    line1 = table.Column<string>(type: "text", nullable: false),
                    postal_code = table.Column<string>(type: "text", nullable: false),
                    coordinates = table.Column<Point>(type: "geometry", nullable: true),
                    number = table.Column<string>(type: "text", nullable: false),
                    street_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_addresses_streets_street_id",
                        column: x => x.street_id,
                        principalTable: "streets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "parks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    address_id = table.Column<int>(type: "integer", nullable: false),
                    entrance_fee = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    organizer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    age_recommendation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, defaultValue: "all"),
                    park_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_free = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parks", x => x.id);
                    table.ForeignKey(
                        name: "FK_parks_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "closing_schedules",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    park_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_closing_schedules", x => x.id);
                    table.ForeignKey(
                        name: "FK_closing_schedules_parks_park_id",
                        column: x => x.park_id,
                        principalTable: "parks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "park_images",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    park_id = table.Column<int>(type: "integer", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_park_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_park_images_parks_park_id",
                        column: x => x.park_id,
                        principalTable: "parks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "closing_schedule_entries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    day = table.Column<string>(type: "text", nullable: false),
                    opens_at = table.Column<string>(type: "text", nullable: false),
                    closes_at = table.Column<string>(type: "text", nullable: false),
                    is_closed = table.Column<bool>(type: "boolean", nullable: false),
                    closing_schedule_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_closing_schedule_entries", x => x.id);
                    table.ForeignKey(
                        name: "FK_closing_schedule_entries_closing_schedules_closing_schedule~",
                        column: x => x.closing_schedule_id,
                        principalTable: "closing_schedules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_addresses_street_id",
                table: "addresses",
                column: "street_id");

            migrationBuilder.CreateIndex(
                name: "IX_closing_schedule_entries_closing_schedule_id",
                table: "closing_schedule_entries",
                column: "closing_schedule_id");

            migrationBuilder.CreateIndex(
                name: "IX_closing_schedules_park_id",
                table: "closing_schedules",
                column: "park_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_georef-spain-municipio-millesime_geom",
                schema: "public",
                table: "georef-spain-municipio-millesime",
                column: "geom")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_georef-spain-municipio-millesime_official_na__6",
                schema: "public",
                table: "georef-spain-municipio-millesime",
                column: "official_na__6");

            migrationBuilder.CreateIndex(
                name: "IX_park_amenities_amenity_id",
                table: "park_amenities",
                column: "amenity_id");

            migrationBuilder.CreateIndex(
                name: "IX_park_amenities_park_id",
                table: "park_amenities",
                column: "park_id");

            migrationBuilder.CreateIndex(
                name: "IX_park_images_park_id",
                table: "park_images",
                column: "park_id");

            migrationBuilder.CreateIndex(
                name: "IX_parks_address_id",
                table: "parks",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_streets_municipio_gid",
                table: "streets",
                column: "municipio_gid");

            migrationBuilder.CreateIndex(
                name: "IX_streets_name_municipio_gid",
                table: "streets",
                columns: new[] { "name", "municipio_gid" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "closing_schedule_entries");

            migrationBuilder.DropTable(
                name: "park_amenities");

            migrationBuilder.DropTable(
                name: "park_images");

            migrationBuilder.DropTable(
                name: "closing_schedules");

            migrationBuilder.DropTable(
                name: "amenities");

            migrationBuilder.DropTable(
                name: "parks");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "streets");

            migrationBuilder.DropTable(
                name: "georef-spain-municipio-millesime",
                schema: "public");
        }
    }
}
