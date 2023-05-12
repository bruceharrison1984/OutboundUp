using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutboundUp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeedTestResults");
        }
    }
}
