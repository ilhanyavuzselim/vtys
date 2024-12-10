using Domain.menu;

namespace Domain.kategori
{
    public class Kategori
    {
        public Guid Id { get; set; }
        public string Ad { get; set; }

        public ICollection<Menu> Menuler { get; set; }
    }

}
