namespace Common.Requests.Personel
{
    public class CreatePersonelRequest
    {
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public Guid? KisiId { get; set; }
        public required string Pozisyon { get; set; }

    }
}
