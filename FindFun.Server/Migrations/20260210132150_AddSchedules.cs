using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FindFun.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "IX_closing_schedule_entries_closing_schedule_id",
                table: "closing_schedule_entries",
                column: "closing_schedule_id");

            migrationBuilder.CreateIndex(
                name: "IX_closing_schedules_park_id",
                table: "closing_schedules",
                column: "park_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "closing_schedule_entries");

            migrationBuilder.DropTable(
                name: "closing_schedules");
        }
    }
}
