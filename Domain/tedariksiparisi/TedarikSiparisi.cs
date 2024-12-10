using Domain.malzeme;
using Domain.tedarikci;

namespace Domain.tedariksiparisi
{
    public class TedarikSiparisi
    {
        public Guid TedarikSiparisiID { get; set; }
        public Guid TedarikciID { get; set; }
        public Guid MalzemeID { get; set; }
        public DateTime SiparisTarihi { get; set; }

        public Tedarikci Tedarikci { get; set; }
        public Malzeme Malzeme { get; set; }
    }

}
