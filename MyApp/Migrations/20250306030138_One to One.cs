using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Migrations
{
    /// <inheritdoc />
    public partial class OnetoOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SerialNumberId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SerialNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SerialNumbers_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ID");
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ID", "Name", "Price", "SerialNumberId" },
                values: new object[] { 1, "Mocrophone", 100.0, 10 });

            migrationBuilder.InsertData(
                table: "SerialNumbers",
                columns: new[] { "Id", "ItemId", "Name" },
                values: new object[] { 10, 1, "MIC150" });

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumbers_ItemId",
                table: "SerialNumbers",
                column: "ItemId",
                unique: true,
                filter: "[ItemId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SerialNumbers");

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "SerialNumberId",
                table: "Items");
        }
    }
}
