using Domain.masa;
using Domain.musteri;
using Domain.personel;
using Domain.siparisdetay;

namespace Domain.siparis
{
    public class Siparis
    {
        public int SiparisID { get; set; }
        public int MasaID { get; set; }
        public int? MusteriID { get; set; }
        public int PersonelID { get; set; }
        public DateTime SiparisTarihi { get; set; } = DateTime.Now;

        public Masa Masa { get; set; }
        public Musteri Musteri { get; set; }
        public Personel Personel { get; set; }
        public ICollection<SiparisDetay> SiparisDetaylar { get; set; }
    }

}
