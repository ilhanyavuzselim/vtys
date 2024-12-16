using Common.Requests.kisi;

namespace Common.Requests.Personel
{
    public class CreatePersonelRequest : CreateKisiRequest
    {
        public Guid KisiId { get; set; }
        public string Pozisyon { get; set; }

    }
}
