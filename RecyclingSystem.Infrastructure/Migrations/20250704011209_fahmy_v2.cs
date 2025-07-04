using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecyclingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fahmy_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PickupRequests_AspNetUsers_EmployeeId",
                table: "PickupRequests");

            migrationBuilder.AlterColumn<int>(
                name: "TotalPointsGiven",
                table: "PickupRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "PickupRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "PickupRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_PickupRequests_AspNetUsers_EmployeeId",
                table: "PickupRequests",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PickupRequests_AspNetUsers_EmployeeId",
                table: "PickupRequests");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "PickupRequests");

            migrationBuilder.AlterColumn<int>(
                name: "TotalPointsGiven",
                table: "PickupRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "PickupRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PickupRequests_AspNetUsers_EmployeeId",
                table: "PickupRequests",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
