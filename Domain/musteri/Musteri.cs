using Domain.kisi;

namespace Domain.musteri
{
    public class Musteri : Kisi
    {
        public Kisi Kisi { get; set; }
        public string Telefon { get; set; }
    }

}
