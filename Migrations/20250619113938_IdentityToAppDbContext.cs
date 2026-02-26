using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StyleSphere.Migrations
{
    /// <inheritdoc />
    public partial class IdentityToAppDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "productSchema");

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "productSchema",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    price = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    image = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    rating = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    buyimage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    isAddedToCart = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product",
                schema: "productSchema");
        }
    }
}
