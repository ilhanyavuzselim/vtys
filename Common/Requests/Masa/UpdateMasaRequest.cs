namespace Common.Requests.Masa
{
    public class UpdateMasaRequest
    {
        public Guid Id { get; set; }
        public int MasaNo { get; set; }
        public int Kapasite { get; set; }
        public bool Durum { get; set; } = false;
    }
}
