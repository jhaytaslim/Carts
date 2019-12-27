using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Carts.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleTb",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(nullable: false),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTb", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "UsersTb",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Profileimage = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    RoleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTb", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UsersTb_RoleTb_RoleId",
                        column: x => x.RoleId,
                        principalTable: "RoleTb",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderTb",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(nullable: false),
                    OrderReference = table.Column<string>(nullable: true),
                    DeliveryAddress = table.Column<string>(nullable: true),
                    DeliveryCost = table.Column<decimal>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateDeleted = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTb", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_OrderTb_UsersTb_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UsersTb",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderTb_UsersTb_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "UsersTb",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductTb",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    FeatureImage = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTb_UsersTb_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UsersTb",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceTb",
                columns: table => new
                {
                    InvoiceId = table.Column<Guid>(nullable: false),
                    TotalOrderCost = table.Column<decimal>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    OrderId = table.Column<Guid>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceTb", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_InvoiceTb_UsersTb_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UsersTb",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceTb_OrderTb_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrderTb",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceTb_ProductTb_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductTb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemTb",
                columns: table => new
                {
                    OrderItemId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: true),
                    PriceSold = table.Column<decimal>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    OrderId = table.Column<Guid>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemTb", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItemTb_UsersTb_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "UsersTb",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemTb_OrderTb_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrderTb",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemTb_ProductTb_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductTb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTb_CreatedById",
                table: "InvoiceTb",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTb_OrderId",
                table: "InvoiceTb",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTb_ProductId",
                table: "InvoiceTb",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemTb_CreatedById",
                table: "OrderItemTb",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemTb_OrderId",
                table: "OrderItemTb",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemTb_ProductId",
                table: "OrderItemTb",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTb_CreatedById",
                table: "OrderTb",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTb_CustomerId",
                table: "OrderTb",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTb_CreatedById",
                table: "ProductTb",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTb_RoleId",
                table: "UsersTb",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceTb");

            migrationBuilder.DropTable(
                name: "OrderItemTb");

            migrationBuilder.DropTable(
                name: "OrderTb");

            migrationBuilder.DropTable(
                name: "ProductTb");

            migrationBuilder.DropTable(
                name: "UsersTb");

            migrationBuilder.DropTable(
                name: "RoleTb");
        }
    }
}
