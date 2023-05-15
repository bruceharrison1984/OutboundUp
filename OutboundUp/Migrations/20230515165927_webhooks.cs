using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutboundUp.Migrations
{
    /// <inheritdoc />
    public partial class webhooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutboundWebHooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TargetUrl = table.Column<string>(type: "TEXT", nullable: false),
                    HttpMethod = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboundWebHooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboundWebHookResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    ResponseCode = table.Column<int>(type: "INTEGER", nullable: false),
                    ResponseBody = table.Column<string>(type: "TEXT", nullable: true),
                    IsSuccess = table.Column<bool>(type: "INTEGER", nullable: false),
                    SpeedTestResultId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboundWebHookResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutboundWebHookResult_OutboundWebHooks_Id",
                        column: x => x.Id,
                        principalTable: "OutboundWebHooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutboundWebHookResult_SpeedTestResults_SpeedTestResultId",
                        column: x => x.SpeedTestResultId,
                        principalTable: "SpeedTestResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutboundWebHookResult_SpeedTestResultId",
                table: "OutboundWebHookResult",
                column: "SpeedTestResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboundWebHookResult");

            migrationBuilder.DropTable(
                name: "OutboundWebHooks");
        }
    }
}
