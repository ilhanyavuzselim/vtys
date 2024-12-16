using Common.Requests.kisi;

namespace Common.Requests.Personel
{
    public class UpdatePersonelRequest : UpdateKisiRequest
    {
        public Guid KisiId { get; set; }
        public string Pozisyon { get; set; }

    }
}
