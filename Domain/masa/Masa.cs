namespace Domain.masa
{
    public class Masa
    {
        public Masa(Guid Id = default, int MasaNo = default, int Kapasite = default, bool Durum = false)
        {
            this.Id = Id;
            this.MasaNo = MasaNo;
            this.Kapasite = Kapasite;
            this.Durum = Durum;
        }
        public Guid Id { get; set; }
        public int MasaNo { get; set; }
        public int Kapasite { get; set; }
        public bool Durum { get; set; } = false; // Varsayılan olarak boş
    }

}
