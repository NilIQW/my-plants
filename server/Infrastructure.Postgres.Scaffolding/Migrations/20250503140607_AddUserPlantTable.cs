using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Postgres.Scaffolding.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPlantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "plants");

            migrationBuilder.CreateTable(
                name: "plant",
                schema: "plants",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    plant_name = table.Column<string>(type: "text", nullable: false),
                    plant_type = table.Column<string>(type: "text", nullable: false),
                    moisture_level = table.Column<float>(type: "real", nullable: false),
                    moisture_threshold = table.Column<float>(type: "real", nullable: false),
                    is_auto_watering_enabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("plant_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "plants",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: false),
                    salt = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_plant",
                schema: "plants",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    plant_id = table.Column<string>(type: "text", nullable: false),
                    is_owner = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_plant_pkey", x => new { x.user_id, x.plant_id });
                    table.ForeignKey(
                        name: "FK_user_plant_plant_plant_id",
                        column: x => x.plant_id,
                        principalSchema: "plants",
                        principalTable: "plant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_plant_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "plants",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "watering_log",
                schema: "plants",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    plant_id = table.Column<string>(type: "text", nullable: false),
                    triggered_by_user_id = table.Column<string>(type: "text", nullable: true),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    method = table.Column<string>(type: "text", nullable: false),
                    moisture_before = table.Column<float>(type: "real", nullable: false),
                    moisture_after = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("watering_log_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_watering_log_plant_plant_id",
                        column: x => x.plant_id,
                        principalSchema: "plants",
                        principalTable: "plant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_watering_log_user_triggered_by_user_id",
                        column: x => x.triggered_by_user_id,
                        principalSchema: "plants",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_plant_plant_id",
                schema: "plants",
                table: "user_plant",
                column: "plant_id");

            migrationBuilder.CreateIndex(
                name: "IX_watering_log_plant_id",
                schema: "plants",
                table: "watering_log",
                column: "plant_id");

            migrationBuilder.CreateIndex(
                name: "IX_watering_log_triggered_by_user_id",
                schema: "plants",
                table: "watering_log",
                column: "triggered_by_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_plant",
                schema: "plants");

            migrationBuilder.DropTable(
                name: "watering_log",
                schema: "plants");

            migrationBuilder.DropTable(
                name: "plant",
                schema: "plants");

            migrationBuilder.DropTable(
                name: "user",
                schema: "plants");
        }
    }
}
