using Domain.kategori;
using System.Text.Json.Serialization;

namespace Domain.menu
{
    public class Menu
    {
        public Guid Id { get; set; }
        public string? Ad { get; set; }
        public decimal Fiyat { get; set; }
        public Guid KategoriID { get; set; }
        [JsonIgnore]
        public Kategori? Kategori { get; set; }
    }

}
