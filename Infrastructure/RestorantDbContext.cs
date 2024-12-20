namespace Infrastructure
{
    using Domain.gider;
    using Domain.kategori;
    using Domain.kisi;
    using Domain.malzeme;
    using Domain.masa;
    using Domain.menu;
    using Domain.musteri;
    using Domain.odeme;
    using Domain.odemeturu;
    using Domain.personel;
    using Domain.rezervasyon;
    using Domain.siparis;
    using Domain.siparisdetay;
    using Domain.stok;
    using Domain.tedarikci;
    using Domain.tedariksiparisi;
    using Microsoft.EntityFrameworkCore;

    namespace Infrastructure.Data
    {
        public class RestorantDbContext : DbContext
        {
            public RestorantDbContext(DbContextOptions<RestorantDbContext> options) : base(options) { }

            public DbSet<Masa> Masalar { get; set; }
            public DbSet<Menu> Menuler { get; set; }
            public DbSet<Kategori> Kategoriler { get; set; }
            public DbSet<Siparis> Siparisler { get; set; }
            public DbSet<SiparisDetay> SiparisDetaylar { get; set; }
            public DbSet<Kisi> Kisiler { get; set; }
            public DbSet<Personel> Personeller { get; set; }
            public DbSet<Musteri> Musteriler { get; set; }
            public DbSet<Rezervasyon> Rezervasyonlar { get; set; }
            public DbSet<Odeme> Odemeler { get; set; }
            public DbSet<OdemeTuru> OdemeTurleri { get; set; }
            public DbSet<Tedarikci> Tedarikciler { get; set; }
            public DbSet<Malzeme> Malzemeler { get; set; }
            public DbSet<Stok> Stoklar { get; set; }
            public DbSet<TedarikSiparisi> TedarikSiparisleri { get; set; }
            public DbSet<Gider> Giderler { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Kisi>()
                    .ToTable("Kisiler");

                modelBuilder.Entity<Personel>()
                    .ToTable("Personeller");

                modelBuilder.Entity<Musteri>()
                    .ToTable("Musteriler");

                modelBuilder.Entity<Personel>()
                    .HasBaseType<Kisi>();

                modelBuilder.Entity<Musteri>()
                    .HasBaseType<Kisi>();
            }
        }
    }
}
