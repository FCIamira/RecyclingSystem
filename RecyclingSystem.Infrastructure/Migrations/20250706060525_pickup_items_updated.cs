using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecyclingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class pickup_items_updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "PickupItems",
                newName: "PlannedQuantity");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Rewards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Rewards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "ActualQuantity",
                table: "PickupItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualQuantity",
                table: "PickupItems");

            migrationBuilder.RenameColumn(
                name: "PlannedQuantity",
                table: "PickupItems",
                newName: "Quantity");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Rewards",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Rewards",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
