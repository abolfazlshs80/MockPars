using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockPars.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renametableidcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Columns");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "Columns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
