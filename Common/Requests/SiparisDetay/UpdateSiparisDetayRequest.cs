namespace Common.Requests.SiparisDetay
{
    public class UpdateSiparisDetayRequest
    {
        public Guid? SiparisID { get; set; }
        public Guid? MenuID { get; set; }
        public int? Adet { get; set; }
    }
}
