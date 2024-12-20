namespace Common.Requests.Odeme
{
    public class CreateOdemeRequest
    {
        public Guid SiparisID { get; set; }
        public Guid OdemeTuruID { get; set; }
        public decimal Tutar { get; set; }
    }
}
