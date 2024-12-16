using Common.Consts;
using Domain.kisi;

namespace Domain.musteri
{
    public class Musteri : Kisi
    {
        public Musteri()
        {
            Discriminator = KisiType.Müşteri.ToString();
        }
        public Guid MusteriId { get; set; }
        public string Telefon { get; set; }  
    }

}
