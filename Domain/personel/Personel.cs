using Common.Consts;
using Domain.kisi;

namespace Domain.personel
{
    public class Personel : Kisi
    {
        public Personel()
        {
            Discriminator = KisiType.Personel.ToString();
        }
        public Personel(Guid id = default, string ad = default, string soyad = default, string pozisyon = default)
        {
            Ad = ad;
            Soyad = soyad;
            Id = id == default ? Guid.NewGuid() : id;
            Pozisyon = pozisyon;
            Discriminator = KisiType.Personel.ToString();
        }
        public Guid PersonelId { get; set; }
        public string Pozisyon { get; set; }
    }

}
