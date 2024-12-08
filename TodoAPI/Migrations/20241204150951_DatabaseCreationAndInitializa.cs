using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TodoAPI.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseCreationAndInitializa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    TodoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsDone = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.TodoId);
                    table.ForeignKey(
                        name: "FK_Todos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Age", "Email", "Name", "Password", "Username" },
                values: new object[,]
                {
                    { new Guid("134103d7-6c6a-4c1f-93e9-eb2f15367ca5"), 28, "boo@boo.com", "MR.Boo", "123456789", "boo" },
                    { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), 18, "foo@foo.com", "MR.Foo", "123456789", "foo" }
                });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "TodoId", "Description", "IsDone", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("6a241878-2e19-4115-982a-984983ad6b96"), "Some Description", false, "Title1", new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870") },
                    { new Guid("6ad8dc78-2f22-477d-9665-cedad4d9880a"), "Some Description 3", true, "Title3", new Guid("134103d7-6c6a-4c1f-93e9-eb2f15367ca5") },
                    { new Guid("a82bdd43-ab9c-48b6-a433-eedc515215ab"), "Some Description 2", false, "Title2", new Guid("134103d7-6c6a-4c1f-93e9-eb2f15367ca5") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_UserId",
                table: "Todos",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
