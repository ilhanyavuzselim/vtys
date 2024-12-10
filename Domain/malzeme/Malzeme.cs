using Domain.tedarikci;

namespace Domain.malzeme
{
    public class Malzeme
    {
        public Guid Id { get; set; }
        public string Ad { get; set; }
        public Guid TedarikciID { get; set; }

        public Tedarikci Tedarikci { get; set; }
    }

}
