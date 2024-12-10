using Domain.odemeturu;
using Domain.siparis;

namespace Domain.odeme
{
    public class Odeme
    {
        public Guid Id { get; set; }
        public Guid SiparisID { get; set; }
        public Guid OdemeTuruID { get; set; }
        public decimal Tutar { get; set; }

        public Siparis Siparis { get; set; }
        public OdemeTuru OdemeTuru { get; set; }
    }

}
