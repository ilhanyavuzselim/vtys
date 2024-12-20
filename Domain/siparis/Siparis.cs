using Domain.masa;
using Domain.musteri;
using Domain.odeme;
using Domain.personel;
using Domain.siparisdetay;
using System.Text.Json.Serialization;

namespace Domain.siparis
{
    public class Siparis
    {
        public Guid Id { get; set; }
        public Guid MasaID { get; set; }
        public Guid? MusteriID { get; set; }
        public Guid PersonelID { get; set; }
        public DateTime SiparisTarihi { get; set; } = DateTime.Now;
        [JsonIgnore]
        public Masa? Masa { get; set; }
        [JsonIgnore]
        public Musteri? Musteri { get; set; }
        [JsonIgnore]
        public Personel? Personel { get; set; }
        public ICollection<SiparisDetay>? SiparisDetaylar { get; set; }
    }

}
