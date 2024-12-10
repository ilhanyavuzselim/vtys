using Domain.masa;

namespace vtysapi.Requests
{
    public class CreateRezervasyonRequest
    {
        public Guid MasaID { get; set; }
        public Guid MusteriID { get; set; }
        public DateTime RezervasyonTarihi { get; set; }

    }
}
