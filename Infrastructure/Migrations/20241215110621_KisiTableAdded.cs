using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class KisiTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Personeller",
                table: "Personeller");

            migrationBuilder.RenameTable(
                name: "Personeller",
                newName: "Kisiler");

            migrationBuilder.AlterColumn<string>(
                name: "Pozisyon",
                table: "Kisiler",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Kisiler",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "Kisiler",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kisiler",
                table: "Kisiler",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kisiler",
                table: "Kisiler");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Kisiler");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Kisiler");

            migrationBuilder.RenameTable(
                name: "Kisiler",
                newName: "Personeller");

            migrationBuilder.AlterColumn<string>(
                name: "Pozisyon",
                table: "Personeller",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Personeller",
                table: "Personeller",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Soyad = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musteriler", x => x.Id);
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
    }
}
