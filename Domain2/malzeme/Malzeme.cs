using Domain.tedarikci;

namespace Domain.malzeme
{
    public class Malzeme
    {
        public int MalzemeID { get; set; }
        public string Ad { get; set; }
        public int TedarikciID { get; set; }

        public Tedarikci Tedarikci { get; set; }
    }

}
