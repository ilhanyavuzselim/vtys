using Domain.masa;
using Domain.musteri;

namespace Domain.rezervasyon
{
    public class Rezervasyon
    {
        public int RezervasyonID { get; set; }
        public int MasaID { get; set; }
        public int MusteriID { get; set; }
        public DateTime RezervasyonTarihi { get; set; }

        public Masa Masa { get; set; }
        public Musteri Musteri { get; set; }
    }

}
