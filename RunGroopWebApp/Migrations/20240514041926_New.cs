using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RunGroopWebApp.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_AppUser_AppUserId",
                table: "Races");

            migrationBuilder.RenameColumn(
                name: "ClubCategorry",
                table: "Clubs",
                newName: "ClubCategory");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Races",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "City", "State", "Street" },
                values: new object[,]
                {
                    { 1, "Charlotte", "NC", "123 Main St" },
                    { 2, "Michigan", "NC", "456 Elm St" }
                });

            migrationBuilder.InsertData(
                table: "Clubs",
                columns: new[] { "Id", "AddressId", "AppUserId", "ClubCategory", "Description", "Image", "Title" },
                values: new object[,]
                {
                    { 1, 1, null, 2, "This is the description of the first cinema", "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360", "Running Club 1" },
                    { 2, 1, null, 4, "Join us for group runs and training sessions", "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360", "Running Club 2" },
                    { 3, 1, null, 3, "This is the description of the first club", "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360", "Running Club 3" },
                    { 4, 2, null, 2, "This is the description of the first club", "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360", "Running Club 4" }
                });

            migrationBuilder.InsertData(
                table: "Races",
                columns: new[] { "Id", "AddressId", "AppUserId", "Description", "Image", "RaceCategory", "Title" },
                values: new object[,]
                {
                    { 1, 1, null, "This is the description of the first race", "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360", 0, "Running Race 1" },
                    { 2, 1, null, "This is the description of the first race", "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360", 1, "Running Race 2" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Races_AppUser_AppUserId",
                table: "Races",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_AppUser_AppUserId",
                table: "Races");

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Races",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Races",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "ClubCategory",
                table: "Clubs",
                newName: "ClubCategorry");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Races",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_AppUser_AppUserId",
                table: "Races",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
