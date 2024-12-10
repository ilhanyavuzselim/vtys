using Domain.masa;
using Domain.musteri;

namespace Domain.rezervasyon
{
    public class Rezervasyon
    {
        private Masa masa;
        private Musteri musteri;

        public Rezervasyon()
        {
            
        }
        public Rezervasyon(Guid MasaId, Guid MusteriId, DateTime RezervasyonTarihi)
        {
            this.MusteriID = MusteriId;
            this.MasaID = MasaID;
            this.RezervasyonTarihi = RezervasyonTarihi;
        }

        public Rezervasyon(Guid MasaId, Guid MusteriId, DateTime RezervasyonTarihi, Masa masa, Musteri musteri) : this(MasaId, MusteriId, RezervasyonTarihi)
        {
            this.masa = masa;
            this.musteri = musteri;
        }

        public Guid Id { get; set; }
        public Guid MasaID { get; set; }
        public Guid MusteriID { get; set; }
        public DateTime RezervasyonTarihi { get; set; }

        public Masa Masa { get; set; }
        public Musteri Musteri { get; set; }
    }

}
