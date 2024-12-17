using Common.Requests.kisi;

namespace Common.Requests.Personel
{
    public class CreatePersonelRequest
    {
        public string? Ad;
        public string? Soyad;
        public Guid? KisiId { get; set; }
        public required string Pozisyon { get; set; }

    }
}
