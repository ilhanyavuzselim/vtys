﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Giderler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Tutar = table.Column<decimal>(type: "numeric", nullable: false),
                    Tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giderler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kategoriler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ad = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoriler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kisiler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Soyad = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kisiler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Masalar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MasaNo = table.Column<int>(type: "integer", nullable: false),
                    Kapasite = table.Column<int>(type: "integer", nullable: false),
                    Durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Masalar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OdemeTurleri",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ad = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdemeTurleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tedarikciler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Iletisim = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tedarikciler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menuler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Fiyat = table.Column<decimal>(type: "numeric", nullable: false),
                    KategoriID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menuler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menuler_Kategoriler_KategoriID",
                        column: x => x.KategoriID,
                        principalTable: "Kategoriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MusteriId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    PersonelId = table.Column<Guid>(type: "uuid", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Malzemeler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    TedarikciID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Malzemeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Malzemeler_Tedarikciler_TedarikciID",
                        column: x => x.TedarikciID,
                        principalTable: "Tedarikciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezervasyonlar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MasaID = table.Column<Guid>(type: "uuid", nullable: false),
                    MusteriID = table.Column<Guid>(type: "uuid", nullable: false),
                    RezervasyonTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervasyonlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezervasyonlar_Masalar_MasaID",
                        column: x => x.MasaID,
                        principalTable: "Masalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervasyonlar_Musteriler_MusteriID",
                        column: x => x.MusteriID,
                        principalTable: "Musteriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Siparisler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MasaID = table.Column<Guid>(type: "uuid", nullable: false),
                    MusteriID = table.Column<Guid>(type: "uuid", nullable: true),
                    PersonelID = table.Column<Guid>(type: "uuid", nullable: false),
                    SiparisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siparisler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Siparisler_Masalar_MasaID",
                        column: x => x.MasaID,
                        principalTable: "Masalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Siparisler_Musteriler_MusteriID",
                        column: x => x.MusteriID,
                        principalTable: "Musteriler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Siparisler_Personeller_PersonelID",
                        column: x => x.PersonelID,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stoklar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MalzemeID = table.Column<Guid>(type: "uuid", nullable: false),
                    Miktar = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stoklar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stoklar_Malzemeler_MalzemeID",
                        column: x => x.MalzemeID,
                        principalTable: "Malzemeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TedarikSiparisleri",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TedarikciID = table.Column<Guid>(type: "uuid", nullable: false),
                    MalzemeID = table.Column<Guid>(type: "uuid", nullable: false),
                    BirimFiyat = table.Column<decimal>(type: "numeric", nullable: false),
                    Miktar = table.Column<decimal>(type: "numeric", nullable: false),
                    SiparisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TedarikSiparisleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TedarikSiparisleri_Malzemeler_MalzemeID",
                        column: x => x.MalzemeID,
                        principalTable: "Malzemeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TedarikSiparisleri_Tedarikciler_TedarikciID",
                        column: x => x.TedarikciID,
                        principalTable: "Tedarikciler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Odemeler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SiparisID = table.Column<Guid>(type: "uuid", nullable: false),
                    OdemeTuruID = table.Column<Guid>(type: "uuid", nullable: false),
                    Tutar = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odemeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odemeler_OdemeTurleri_OdemeTuruID",
                        column: x => x.OdemeTuruID,
                        principalTable: "OdemeTurleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Odemeler_Siparisler_SiparisID",
                        column: x => x.SiparisID,
                        principalTable: "Siparisler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiparisDetaylar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SiparisID = table.Column<Guid>(type: "uuid", nullable: false),
                    MenuID = table.Column<Guid>(type: "uuid", nullable: false),
                    Adet = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisDetaylar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiparisDetaylar_Menuler_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menuler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiparisDetaylar_Siparisler_SiparisID",
                        column: x => x.SiparisID,
                        principalTable: "Siparisler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Malzemeler_TedarikciID",
                table: "Malzemeler",
                column: "TedarikciID");

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_KategoriID",
                table: "Menuler",
                column: "KategoriID");

            migrationBuilder.CreateIndex(
                name: "IX_Odemeler_OdemeTuruID",
                table: "Odemeler",
                column: "OdemeTuruID");

            migrationBuilder.CreateIndex(
                name: "IX_Odemeler_SiparisID",
                table: "Odemeler",
                column: "SiparisID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_MasaID",
                table: "Rezervasyonlar",
                column: "MasaID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervasyonlar_MusteriID",
                table: "Rezervasyonlar",
                column: "MusteriID");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisDetaylar_MenuID",
                table: "SiparisDetaylar",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisDetaylar_SiparisID",
                table: "SiparisDetaylar",
                column: "SiparisID");

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_MasaID",
                table: "Siparisler",
                column: "MasaID");

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_MusteriID",
                table: "Siparisler",
                column: "MusteriID");

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_PersonelID",
                table: "Siparisler",
                column: "PersonelID");

            migrationBuilder.CreateIndex(
                name: "IX_Stoklar_MalzemeID",
                table: "Stoklar",
                column: "MalzemeID");

            migrationBuilder.CreateIndex(
                name: "IX_TedarikSiparisleri_MalzemeID",
                table: "TedarikSiparisleri",
                column: "MalzemeID");

            migrationBuilder.CreateIndex(
                name: "IX_TedarikSiparisleri_TedarikciID",
                table: "TedarikSiparisleri",
                column: "TedarikciID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Giderler");

            migrationBuilder.DropTable(
                name: "Odemeler");

            migrationBuilder.DropTable(
                name: "Rezervasyonlar");

            migrationBuilder.DropTable(
                name: "SiparisDetaylar");

            migrationBuilder.DropTable(
                name: "Stoklar");

            migrationBuilder.DropTable(
                name: "TedarikSiparisleri");

            migrationBuilder.DropTable(
                name: "OdemeTurleri");

            migrationBuilder.DropTable(
                name: "Menuler");

            migrationBuilder.DropTable(
                name: "Siparisler");

            migrationBuilder.DropTable(
                name: "Malzemeler");

            migrationBuilder.DropTable(
                name: "Kategoriler");

            migrationBuilder.DropTable(
                name: "Masalar");

            migrationBuilder.DropTable(
                name: "Musteriler");

            migrationBuilder.DropTable(
                name: "Personeller");

            migrationBuilder.DropTable(
                name: "Tedarikciler");

            migrationBuilder.DropTable(
                name: "Kisiler");
        }
    }
}
