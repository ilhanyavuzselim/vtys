namespace Common.Requests.Menu
{
    public class CreateMenuRequest
    {
        public required string Ad { get; set; }
        public required decimal Fiyat { get; set; }
        public required Guid KategoriID { get; set; }
    }
}
