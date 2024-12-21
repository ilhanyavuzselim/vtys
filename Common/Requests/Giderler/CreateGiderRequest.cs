namespace Common.Requests.Giderler
{
    public class CreateGiderRequest
    {
        public required string Ad { get; set; }
        public required decimal Tutar { get; set; }
        public required DateTime Tarih { get; set; }
    }
}
