using Domain.masa;
using Domain.musteri;

namespace Domain.rezervasyon
{
    public class Rezervasyon
    {
        public Guid RezervasyonID { get; set; }
        public Guid MasaID { get; set; }
        public Guid MusteriID { get; set; }
        public DateTime RezervasyonTarihi { get; set; }

        public Masa Masa { get; set; }
        public Musteri Musteri { get; set; }
    }

}
