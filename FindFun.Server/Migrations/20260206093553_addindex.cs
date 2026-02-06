using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindFun.Server.Migrations
{
    /// <inheritdoc />
    public partial class addindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "local_name_",
                schema: "public",
                table: "georef-spain-municipio-millesime",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_georef-spain-municipio-millesime_geom",
                schema: "public",
                table: "georef-spain-municipio-millesime");

            migrationBuilder.DropIndex(
                name: "IX_georef-spain-municipio-millesime_official_na__6",
                schema: "public",
                table: "georef-spain-municipio-millesime");

            migrationBuilder.AlterColumn<string>(
                name: "local_name_",
                schema: "public",
                table: "georef-spain-municipio-millesime",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
