using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FindFun.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

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
                    local_name_ = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
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
                    street_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_addresses_streets_street_id",
                        column: x => x.street_id,
                        principalTable: "streets",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "parks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    address_id = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_addresses_street_id",
                table: "addresses",
                column: "street_id");

            migrationBuilder.CreateIndex(
                name: "IX_parks_address_id",
                table: "parks",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_streets_municipio_gid",
                table: "streets",
                column: "municipio_gid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
