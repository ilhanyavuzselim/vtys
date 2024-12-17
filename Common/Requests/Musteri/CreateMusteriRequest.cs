using System.ComponentModel.DataAnnotations;

namespace Common.Requests.Musteri
{
    public class CreateMusteriRequest
    {
        public string? Ad {  get; set; }
        public string? Soyad { get; set; }
        public required string Telefon { get; set; }
        public Guid? KisiId { get; set; }
    }
}
