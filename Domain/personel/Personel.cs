using Domain.kisi;

namespace Domain.personel
{
    public class Personel : Kisi
    {
        public Kisi Kisi { get; set; }
        public string Pozisyon { get; set; }
    }

}
