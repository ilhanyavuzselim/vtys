using Domain.menu;
using Domain.siparis;

namespace Domain.siparisdetay
{
    public class SiparisDetay
    {
        public int SiparisDetayID { get; set; }
        public int SiparisID { get; set; }
        public int MenuID { get; set; }
        public int Adet { get; set; }

        public Siparis Siparis { get; set; }
        public Menu Menu { get; set; }
    }

}
