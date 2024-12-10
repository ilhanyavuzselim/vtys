using Domain.malzeme;
using Domain.tedarikci;

namespace Domain.tedariksiparisi
{
    public class TedarikSiparisi
    {
        public int TedarikSiparisiID { get; set; }
        public int TedarikciID { get; set; }
        public int MalzemeID { get; set; }
        public DateTime SiparisTarihi { get; set; }

        public Tedarikci Tedarikci { get; set; }
        public Malzeme Malzeme { get; set; }
    }

}
