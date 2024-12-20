namespace Common.Requests.TedarikSiparisi
{
    public class UpdateTedarikSiparisiRequest
    {
        public Guid TedarikciID { get; set; }
        public Guid MalzemeID { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal Miktar { get; set; }
        public DateTime SiparisTarihi { get; set; }
    }
}
