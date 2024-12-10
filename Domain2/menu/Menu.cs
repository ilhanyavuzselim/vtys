using Domain.kategori;

namespace Domain.menu
{
    public class Menu
    {
        public int MenuID { get; set; }
        public string Ad { get; set; }
        public decimal Fiyat { get; set; }
        public int KategoriID { get; set; }

        public Kategori Kategori { get; set; }
    }

}
