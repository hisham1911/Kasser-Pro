using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KasserPro.Migrations
{
    /// <inheritdoc />
    public partial class InitialMultiTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    Logo = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaxEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    TaxRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    StoreName = table.Column<string>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    StoreId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSettings_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    Icon = table.Column<string>(type: "TEXT", nullable: true),
                    StoreId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StoreId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    StoreId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderNumber = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Subtotal = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    TaxRate = table.Column<decimal>(type: "TEXT", precision: 5, scale: 4, nullable: false),
                    PaymentMethod = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    StoreId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    PriceAtTime = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "Address", "CreatedAt", "ExpiresAt", "IsActive", "Logo", "Name", "Phone" },
                values: new object[] { 1, "القاهرة، مصر", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, "متجر تجريبي", "01000000000" });

            migrationBuilder.InsertData(
                table: "AppSettings",
                columns: new[] { "Id", "Currency", "StoreId", "StoreName", "TaxEnabled", "TaxRate" },
                values: new object[] { 1, "ج.م", 1, "KasserPro", true, 14m });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Color", "Icon", "Name", "StoreId" },
                values: new object[,]
                {
                    { 1, "#3B82F6", "🥤", "مشروبات", 1 },
                    { 2, "#EF4444", "🍔", "وجبات", 1 },
                    { 3, "#F59E0B", "🍰", "حلويات", 1 },
                    { 4, "#10B981", "🍟", "مقبلات", 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "FullName", "IsActive", "PasswordHash", "Role", "StoreId", "Username" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "المدير العام", true, "$2a$11$5EqkFvM3Y8LZZ0YHqVn4ZeOxNc7DWYpCvLZU3p6qWGNJxJ8HK9EzS", "Owner", 1, "admin" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "ImageUrl", "IsAvailable", "Name", "Price", "Stock", "StoreId" },
                values: new object[,]
                {
                    { 1, 1, null, true, "كولا", 15.00m, 100, 1 },
                    { 2, 1, null, true, "عصير برتقال", 20.00m, 50, 1 },
                    { 3, 1, null, true, "مياه معدنية", 10.00m, 200, 1 },
                    { 4, 2, null, true, "برجر لحم", 50.00m, 30, 1 },
                    { 5, 2, null, true, "برجر دجاج", 45.00m, 35, 1 },
                    { 6, 2, null, true, "بيتزا مارجريتا", 80.00m, 20, 1 },
                    { 7, 3, null, true, "كيكة شوكولاتة", 25.00m, 15, 1 },
                    { 8, 3, null, true, "آيس كريم", 30.00m, 40, 1 },
                    { 9, 4, null, true, "بطاطس", 20.00m, 60, 1 },
                    { 10, 4, null, true, "أصابع موتزاريلا", 35.00m, 25, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSettings_StoreId",
                table: "AppSettings",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_StoreId",
                table: "Categories",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedAt",
                table: "Orders",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreId",
                table: "Orders",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId",
                table: "Products",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_StoreId",
                table: "Users",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
