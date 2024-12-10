using Domain.malzeme;

namespace Domain.stok
{
    public class Stok
    {
        public Guid StokID { get; set; }
        public Guid MalzemeID { get; set; }
        public int Miktar { get; set; }

        public Malzeme Malzeme { get; set; }
    }

}
