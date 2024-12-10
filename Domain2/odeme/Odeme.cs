using Domain.odemeturu;
using Domain.siparis;

namespace Domain.odeme
{
    public class Odeme
    {
        public int OdemeID { get; set; }
        public int SiparisID { get; set; }
        public int OdemeTuruID { get; set; }
        public decimal Tutar { get; set; }

        public Siparis Siparis { get; set; }
        public OdemeTuru OdemeTuru { get; set; }
    }

}
