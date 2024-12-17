namespace Common.Requests.Menu
{
    public class UpdateMenuRequest
    {
        public string? Ad {  get; set; }
        public decimal? Fiyat { get; set; }
        public Guid? KategoriID { get; set; }
    }
}
