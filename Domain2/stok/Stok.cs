using Domain.malzeme;

namespace Domain.stok
{
    public class Stok
    {
        public int StokID { get; set; }
        public int MalzemeID { get; set; }
        public int Miktar { get; set; }

        public Malzeme Malzeme { get; set; }
    }

}
