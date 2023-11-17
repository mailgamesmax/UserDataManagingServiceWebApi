using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserDataManagingService.Migrations
{
    /// <inheritdoc />
    public partial class firstInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PersonalCode = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(350)", maxLength: 350, nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LivingPlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "LivingPlaces",
                columns: table => new
                {
                    LivingPlace_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Miestas = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Gatve = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Namonr = table.Column<string>(name: "Namo nr", type: "nvarchar(max)", nullable: false),
                    Butonr = table.Column<string>(name: "Buto nr", type: "nvarchar(15)", maxLength: 15, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivingPlaces", x => x.LivingPlace_Id);
                    table.ForeignKey(
                        name: "FK_LivingPlaces_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LivingPlaces_UserId",
                table: "LivingPlaces",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_NickName",
                table: "Users",
                column: "NickName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LivingPlaces");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
