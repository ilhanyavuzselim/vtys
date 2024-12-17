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
        public Guid PersonelId { get; set; }
        public string? Pozisyon { get; set; }
    }

}
