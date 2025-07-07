using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecyclingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_PickupRequests_PickupRequestId",
                table: "Reports");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PickupRequestId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_PickupRequests_PickupRequestId",
                table: "Reports",
                column: "PickupRequestId",
                principalTable: "PickupRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_PickupRequests_PickupRequestId",
                table: "Reports");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Reports",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PickupRequestId",
                table: "Reports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_PickupRequests_PickupRequestId",
                table: "Reports",
                column: "PickupRequestId",
                principalTable: "PickupRequests",
                principalColumn: "Id");
        }
    }
}
