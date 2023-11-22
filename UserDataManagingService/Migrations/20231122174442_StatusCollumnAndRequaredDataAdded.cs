using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserDataManagingService.Migrations
{
    /// <inheritdoc />
    public partial class StatusCollumnAndRequaredDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNr",
                table: "Users",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(350)",
                oldMaxLength: 350);

            migrationBuilder.AddColumn<bool>(
                name: "UserIsActive",
                table: "Users",
                type: "bit",
                maxLength: 350,
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Namo nr",
                table: "LivingPlaces",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIsActive",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNr",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "varbinary(350)",
                maxLength: 350,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Namo nr",
                table: "LivingPlaces",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);
        }
    }
}
