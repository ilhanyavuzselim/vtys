using Domain.menu;
using Domain.siparis;

namespace Domain.siparisdetay
{
    public class SiparisDetay
    {
        public Guid Id { get; set; }
        public Guid SiparisID { get; set; }
        public Guid MenuID { get; set; }
        public int Adet { get; set; }
        public Siparis? Siparis { get; set; }
        public Menu? Menu { get; set; }
    }

}
