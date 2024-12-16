using Common.Consts;

namespace Domain.kisi
{
    public class Kisi
    {
        public Kisi(Guid Id = default, string Ad = "", string Soyad = "")
        {
            this.Id = Id;
            this.Ad = Ad;
            this.Soyad = Soyad;
        }
        public Guid Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Discriminator { get; set; } = KisiType.Kişi.ToString();
    }
}
