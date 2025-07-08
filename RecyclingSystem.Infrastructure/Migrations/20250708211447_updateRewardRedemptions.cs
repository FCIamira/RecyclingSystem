using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecyclingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRewardRedemptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "RewardRedemptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPoints",
                table: "RewardRedemptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "RewardRedemptions");

            migrationBuilder.DropColumn(
                name: "TotalPoints",
                table: "RewardRedemptions");

        

          
        }
    }
}
