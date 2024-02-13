using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_RKonnect.Migrations
{
    /// <inheritdoc />
    public partial class updateUserPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Utilisateur",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Utilisateur",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Utilisateur");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Utilisateur");
        }
    }
}
