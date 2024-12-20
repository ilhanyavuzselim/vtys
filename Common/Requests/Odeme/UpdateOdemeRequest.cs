namespace Common.Requests.Odeme
{
    public class UpdateOdemeRequest
    {
        public Guid SiparisID { get; set; }
        public Guid OdemeTuruID { get; set; }
        public decimal Tutar { get; set; }
    }
}
