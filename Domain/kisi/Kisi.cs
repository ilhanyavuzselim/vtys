using System.Text.Json.Serialization;

namespace Domain.kisi
{
    public class Kisi
    {
        public Guid Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        [JsonIgnore]
        public string? Discriminator { get; set; }
    }
}
