using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockPars.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRowIndextablerecordData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RowIndex",
                table: "RecordData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowIndex",
                table: "RecordData");
        }
    }
}
