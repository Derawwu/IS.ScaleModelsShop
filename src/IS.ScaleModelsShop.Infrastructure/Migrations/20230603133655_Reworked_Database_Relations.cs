using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IS.ScaleModelsShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Reworked_Database_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Category_LinkedCategoryId",
                table: "ProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Products_LinkedProductId",
                table: "ProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "newId()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Manufacturers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "newId()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "newId()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_LinkedCategoryId",
                table: "ProductCategory",
                column: "LinkedCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Categories_LinkedCategoryId",
                table: "ProductCategory",
                column: "LinkedCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Products_LinkedProductId",
                table: "ProductCategory",
                column: "LinkedProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Categories_LinkedCategoryId",
                table: "ProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Products_LinkedProductId",
                table: "ProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategory_LinkedCategoryId",
                table: "ProductCategory");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newId()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Manufacturers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newId()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newId()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory",
                columns: new[] { "LinkedCategoryId", "LinkedProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Category_LinkedCategoryId",
                table: "ProductCategory",
                column: "LinkedCategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Products_LinkedProductId",
                table: "ProductCategory",
                column: "LinkedProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
