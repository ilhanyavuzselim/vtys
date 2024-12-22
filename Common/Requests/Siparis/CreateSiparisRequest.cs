using Common.Requests.SiparisDetay;

namespace Common.Requests.Siparis
{
    public class CreateSiparisRequest
    {
        public Guid MasaID { get; set; }
        public Guid MusteriID { get; set; }
        public Guid PersonelID { get; set; }
        public DateTime SiparisTarihi { get; set; } = DateTime.Now;
        public List<CreateSiparisDetayRequest> SiparisDetaylar { get; set; } = new List<CreateSiparisDetayRequest>(); 
    }
}
