namespace Common.Requests.Masa
{
    public class CreateMasaRequest
    {
        public int MasaNo { get; set; }
        public int Kapasite { get; set; }
        public bool Durum { get; set; } = false;
    }
}
