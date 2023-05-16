using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutboundUp.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                    TargetUrl = table.Column<string>(type: "TEXT", nullable: true),
                    HttpMethod = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboundWebHooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpeedTestResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnixTimestampMs = table.Column<long>(type: "INTEGER", nullable: false),
                    IsSuccess = table.Column<bool>(type: "INTEGER", nullable: false),
                    ServerHostName = table.Column<string>(type: "TEXT", nullable: true),
                    UploadLatencyAverage = table.Column<double>(type: "REAL", nullable: false),
                    UploadLatencyHigh = table.Column<double>(type: "REAL", nullable: false),
                    UploadLatencyLow = table.Column<double>(type: "REAL", nullable: false),
                    DownloadLatencyAverage = table.Column<double>(type: "REAL", nullable: false),
                    DownloadLatencyHigh = table.Column<double>(type: "REAL", nullable: false),
                    DownloadLatencyLow = table.Column<double>(type: "REAL", nullable: false),
                    PingAverage = table.Column<double>(type: "REAL", nullable: false),
                    PingHigh = table.Column<double>(type: "REAL", nullable: false),
                    PingLow = table.Column<double>(type: "REAL", nullable: false),
                    DownloadSpeed = table.Column<double>(type: "REAL", nullable: false),
                    UploadSpeed = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeedTestResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboundWebHookResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResponseCode = table.Column<int>(type: "INTEGER", nullable: false),
                    ResponseBody = table.Column<string>(type: "TEXT", nullable: true),
                    IsSuccess = table.Column<bool>(type: "INTEGER", nullable: false),
                    WebhookId = table.Column<int>(type: "INTEGER", nullable: false),
                    SpeedTestResultId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboundWebHookResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutboundWebHookResult_OutboundWebHooks_WebhookId",
                        column: x => x.WebhookId,
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

            migrationBuilder.CreateIndex(
                name: "IX_OutboundWebHookResult_WebhookId",
                table: "OutboundWebHookResult",
                column: "WebhookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboundWebHookResult");

            migrationBuilder.DropTable(
                name: "OutboundWebHooks");

            migrationBuilder.DropTable(
                name: "SpeedTestResults");
        }
    }
}
