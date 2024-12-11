namespace Common.Requests.Rezervasyon
{
    public class UpdateRezervasyonRequest
    {
        public Guid Id { get; set; }
        public Guid MasaID { get; set; }
        public Guid MusteriID { get; set; }
        public DateTime RezervasyonTarihi { get; set; }
    }
}
