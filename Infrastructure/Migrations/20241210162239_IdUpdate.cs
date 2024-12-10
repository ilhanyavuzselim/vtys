using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IdUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TedarikSiparisiID",
                table: "TedarikSiparisleri",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TedarikciID",
                table: "Tedarikciler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "StokID",
                table: "Stoklar",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SiparisID",
                table: "Siparisler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SiparisDetayID",
                table: "SiparisDetaylar",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "RezervasyonID",
                table: "Rezervasyonlar",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PersonelID",
                table: "Personeller",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "OdemeTuruID",
                table: "OdemeTurleri",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "OdemeID",
                table: "Odemeler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MusteriID",
                table: "Musteriler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MenuID",
                table: "Menuler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MasaID",
                table: "Masalar",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MalzemeID",
                table: "Malzemeler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "KategoriID",
                table: "Kategoriler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "GiderID",
                table: "Giderler",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TedarikSiparisleri",
                newName: "TedarikSiparisiID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tedarikciler",
                newName: "TedarikciID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Stoklar",
                newName: "StokID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Siparisler",
                newName: "SiparisID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SiparisDetaylar",
                newName: "SiparisDetayID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Rezervasyonlar",
                newName: "RezervasyonID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Personeller",
                newName: "PersonelID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OdemeTurleri",
                newName: "OdemeTuruID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Odemeler",
                newName: "OdemeID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Musteriler",
                newName: "MusteriID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Menuler",
                newName: "MenuID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Masalar",
                newName: "MasaID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Malzemeler",
                newName: "MalzemeID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Kategoriler",
                newName: "KategoriID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Giderler",
                newName: "GiderID");
        }
    }
}
