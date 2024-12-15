using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class KisiTableHierarchyUpdatev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervasyonlar_Kisiler_MusteriID",
                table: "Rezervasyonlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_Kisiler_MusteriID",
                table: "Siparisler");

            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_Kisiler_PersonelID",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Kisiler");

            migrationBuilder.DropColumn(
                name: "Pozisyon",
                table: "Kisiler");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Kisiler");

            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musteriler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Musteriler_Kisiler_Id",
                        column: x => x.Id,
                        principalTable: "Kisiler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Personeller",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Pozisyon = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personeller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personeller_Kisiler_Id",
                        column: x => x.Id,
                        principalTable: "Kisiler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervasyonlar_Musteriler_MusteriID",
                table: "Rezervasyonlar",
                column: "MusteriID",
                principalTable: "Musteriler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_Musteriler_MusteriID",
                table: "Siparisler",
                column: "MusteriID",
                principalTable: "Musteriler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_Personeller_PersonelID",
                table: "Siparisler",
                column: "PersonelID",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervasyonlar_Musteriler_MusteriID",
                table: "Rezervasyonlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_Musteriler_MusteriID",
                table: "Siparisler");

            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_Personeller_PersonelID",
                table: "Siparisler");

            migrationBuilder.DropTable(
                name: "Musteriler");

            migrationBuilder.DropTable(
                name: "Personeller");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Kisiler",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pozisyon",
                table: "Kisiler",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "Kisiler",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervasyonlar_Kisiler_MusteriID",
                table: "Rezervasyonlar",
                column: "MusteriID",
                principalTable: "Kisiler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_Kisiler_MusteriID",
                table: "Siparisler",
                column: "MusteriID",
                principalTable: "Kisiler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_Kisiler_PersonelID",
                table: "Siparisler",
                column: "PersonelID",
                principalTable: "Kisiler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
