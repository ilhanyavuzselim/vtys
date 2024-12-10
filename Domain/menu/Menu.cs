using Domain.kategori;

namespace Domain.menu
{
    public class Menu
    {
        public Guid MenuID { get; set; }
        public string Ad { get; set; }
        public decimal Fiyat { get; set; }
        public Guid KategoriID { get; set; }

        public Kategori Kategori { get; set; }
    }

}
