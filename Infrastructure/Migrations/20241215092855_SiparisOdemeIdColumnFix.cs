using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SiparisOdemeIdColumnFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Odemeler_Siparisler_SiparisID",
                table: "Odemeler");

            migrationBuilder.DropIndex(
                name: "IX_Odemeler_SiparisID",
                table: "Odemeler");

            migrationBuilder.AddColumn<Guid>(
                name: "OdemeID",
                table: "Siparisler",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_OdemeID",
                table: "Siparisler",
                column: "OdemeID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_Odemeler_OdemeID",
                table: "Siparisler",
                column: "OdemeID",
                principalTable: "Odemeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_Odemeler_OdemeID",
                table: "Siparisler");

            migrationBuilder.DropIndex(
                name: "IX_Siparisler_OdemeID",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "OdemeID",
                table: "Siparisler");

            migrationBuilder.CreateIndex(
                name: "IX_Odemeler_SiparisID",
                table: "Odemeler",
                column: "SiparisID");

            migrationBuilder.AddForeignKey(
                name: "FK_Odemeler_Siparisler_SiparisID",
                table: "Odemeler",
                column: "SiparisID",
                principalTable: "Siparisler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
