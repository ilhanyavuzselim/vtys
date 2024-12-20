﻿namespace Common.Requests.Siparis
{
    public class UpdateSiparisRequest
    {
        public Guid? MasaID { get; set; }
        public Guid? MusteriID { get; set; }
        public Guid? PersonelID { get; set; }
        public DateTime? SiparisTarihi { get; set; } = DateTime.Now;
    }
}
