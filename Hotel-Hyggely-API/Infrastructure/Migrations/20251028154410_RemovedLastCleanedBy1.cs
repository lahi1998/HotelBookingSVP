using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedLastCleanedBy1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Staff_LastCleanedById",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_LastCleanedById",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "LastCleanedById",
                table: "Rooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastCleanedById",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_LastCleanedById",
                table: "Rooms",
                column: "LastCleanedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Staff_LastCleanedById",
                table: "Rooms",
                column: "LastCleanedById",
                principalTable: "Staff",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
