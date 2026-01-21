using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace REST_API.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Interests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonInterestLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    InterestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonInterestLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonInterestLinks_Interests_InterestId",
                        column: x => x.InterestId,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonInterestLinks_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Interests",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "All about programming and software development.", "Coding" },
                    { 2, "Exploring new places and cultures around the world.", "Traveling" },
                    { 3, "Capturing moments through the lens.", "Photography" },
                    { 4, "Creating delicious meals and culinary experiences.", "Cooking" },
                    { 5, "Enjoying and creating music across various genres.", "Music" },
                    { 6, "Participating in and watching various sports activities.", "Sports" },
                    { 7, "Diving into books and expanding knowledge.", "Reading" },
                    { 8, "Engaging in video games and interactive entertainment.", "Gaming" },
                    { 9, "Exploring nature through hiking and outdoor adventures.", "Hiking" },
                    { 10, "Creating and appreciating various forms of art.", "Art" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "John", "Doe", "123-456-7890" },
                    { 2, "Jane", "Smith", "987-654-3210" },
                    { 3, "Alice", "Johnson", "555-123-4567" },
                    { 4, "Bob", "Brown", "444-987-6543" },
                    { 5, "Charlie", "Davis", "333-222-1111" },
                    { 6, "Eve", "Wilson", "777-888-9999" },
                    { 7, "Frank", "Miller", "666-555-4444" },
                    { 8, "Grace", "Taylor", "222-333-4444" },
                    { 9, "Hank", "Anderson", "111-222-3333" },
                    { 10, "Ivy", "Thomas", "888-777-6666" }
                });

            migrationBuilder.InsertData(
                table: "PersonInterestLinks",
                columns: new[] { "Id", "InterestId", "PersonId", "Url" },
                values: new object[,]
                {
                    { 1, 1, 1, "https://example.com/johns-coding-blog" },
                    { 2, 2, 2, "https://example.com/janes-travel-vlog" },
                    { 3, 3, 3, "https://example.com/alices-photo-gallery" },
                    { 4, 4, 4, "https://example.com/bobs-cooking-recipes" },
                    { 5, 5, 5, "https://example.com/charlies-music-playlist" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonInterestLinks_InterestId",
                table: "PersonInterestLinks",
                column: "InterestId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonInterestLinks_PersonId",
                table: "PersonInterestLinks",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonInterestLinks");

            migrationBuilder.DropTable(
                name: "Interests");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
