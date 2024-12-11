namespace MVC
{
    public static class ApiEndpoints
    {
        // API Base URL
        public static string ApiBaseUrl { get; private set; } = string.Empty;

        // API'yi başlatan metot
        public static void Initialize(IConfiguration configuration)
        {
            var url = configuration.GetSection("ApiSettings:BaseUrl").Value + "/api";
            if (url != null)
            {
                ApiBaseUrl = url;
            }
        }

        // GiderController URL'leri
        public static string GiderControllerBaseUrl => $"{ApiBaseUrl}/Gider";
        public static string GiderControllerGetUrl => GiderControllerBaseUrl;
        public static string GiderControllerGetByIdUrl => $"{GiderControllerBaseUrl}/";
        public static string GiderControllerCreateUrl =>GiderControllerBaseUrl;
        public static string GiderControllerUpdateUrl =>$"{GiderControllerBaseUrl}/";
        public static string GiderControllerDeleteUrl =>$"{GiderControllerBaseUrl}/";

        // KategoriController URL'leri
        public static string KategoriControllerBaseUrl =>$"{ApiBaseUrl}/Kategori";
        public static string KategoriControllerGetUrl =>KategoriControllerBaseUrl;
        public static string KategoriControllerGetByIdUrl =>$"{KategoriControllerBaseUrl}/";
        public static string KategoriControllerCreateUrl =>KategoriControllerBaseUrl;
        public static string KategoriControllerUpdateUrl =>$"{KategoriControllerBaseUrl}/";
        public static string KategoriControllerDeleteUrl =>$"{KategoriControllerBaseUrl}/";

        // MalzemeController URL'leri
        public static string MalzemeControllerBaseUrl =>$"{ApiBaseUrl}/Malzeme";
        public static string MalzemeControllerGetUrl =>MalzemeControllerBaseUrl;
        public static string MalzemeControllerGetByIdUrl =>$"{MalzemeControllerBaseUrl}/";
        public static string MalzemeControllerCreateUrl =>MalzemeControllerBaseUrl;
        public static string MalzemeControllerUpdateUrl =>$"{MalzemeControllerBaseUrl}/";
        public static string MalzemeControllerDeleteUrl =>$"{MalzemeControllerBaseUrl}/";

        // MasaController URL'leri
        public static string MasaControllerBaseUrl =>$"{ApiBaseUrl}/Masa";
        public static string MasaControllerGetUrl =>MasaControllerBaseUrl;
        public static string MasaControllerGetByIdUrl =>$"{MasaControllerBaseUrl}/";
        public static string MasaControllerCreateUrl =>MasaControllerBaseUrl;
        public static string MasaControllerUpdateUrl =>$"{MasaControllerBaseUrl}/";
        public static string MasaControllerDeleteUrl =>$"{MasaControllerBaseUrl}/";

        // MenuController URL'leri
        public static string MenuControllerBaseUrl =>$"{ApiBaseUrl}/Menu";
        public static string MenuControllerGetUrl =>MenuControllerBaseUrl;
        public static string MenuControllerGetByIdUrl =>$"{MenuControllerBaseUrl}/";
        public static string MenuControllerCreateUrl =>MenuControllerBaseUrl;
        public static string MenuControllerUpdateUrl =>$"{MenuControllerBaseUrl}/";
        public static string MenuControllerDeleteUrl =>$"{MenuControllerBaseUrl}/";

        // MusteriController URL'leri
        public static string MusteriControllerBaseUrl =>$"{ApiBaseUrl}/Musteri";
        public static string MusteriControllerGetUrl =>MusteriControllerBaseUrl;
        public static string MusteriControllerGetByIdUrl =>$"{MusteriControllerBaseUrl}/";
        public static string MusteriControllerCreateUrl =>MusteriControllerBaseUrl;
        public static string MusteriControllerUpdateUrl =>$"{MusteriControllerBaseUrl}/";
        public static string MusteriControllerDeleteUrl =>$"{MusteriControllerBaseUrl}/";

        // OdemeController URL'leri
        public static string OdemeControllerBaseUrl =>$"{ApiBaseUrl}/Odeme";
        public static string OdemeControllerGetUrl =>OdemeControllerBaseUrl;
        public static string OdemeControllerGetByIdUrl =>$"{OdemeControllerBaseUrl}/";
        public static string OdemeControllerCreateUrl =>OdemeControllerBaseUrl;
        public static string OdemeControllerUpdateUrl =>$"{OdemeControllerBaseUrl}/";
        public static string OdemeControllerDeleteUrl =>$"{OdemeControllerBaseUrl}/";

        // OdemeTuruController URL'leri
        public static string OdemeTuruControllerBaseUrl =>$"{ApiBaseUrl}/OdemeTuru";
        public static string OdemeTuruControllerGetUrl =>OdemeTuruControllerBaseUrl;
        public static string OdemeTuruControllerGetByIdUrl =>$"{OdemeTuruControllerBaseUrl}/";
        public static string OdemeTuruControllerCreateUrl =>OdemeTuruControllerBaseUrl;
        public static string OdemeTuruControllerUpdateUrl =>$"{OdemeTuruControllerBaseUrl}/";
        public static string OdemeTuruControllerDeleteUrl =>$"{OdemeTuruControllerBaseUrl}/";

        // PersonelController URL'leri
        public static string PersonelControllerBaseUrl =>$"{ApiBaseUrl}/Personel";
        public static string PersonelControllerGetUrl =>PersonelControllerBaseUrl;
        public static string PersonelControllerGetByIdUrl =>$"{PersonelControllerBaseUrl}/";
        public static string PersonelControllerCreateUrl =>PersonelControllerBaseUrl;
        public static string PersonelControllerUpdateUrl =>$"{PersonelControllerBaseUrl}/";
        public static string PersonelControllerDeleteUrl =>$"{PersonelControllerBaseUrl}/";

        // RezervasyonController URL'leri
        public static string RezervasyonControllerBaseUrl =>$"{ApiBaseUrl}/Rezervasyon";
        public static string RezervasyonControllerGetUrl =>RezervasyonControllerBaseUrl;
        public static string RezervasyonControllerGetByIdUrl =>$"{RezervasyonControllerBaseUrl}/";
        public static string RezervasyonControllerCreateUrl =>RezervasyonControllerBaseUrl;
        public static string RezervasyonControllerUpdateUrl =>$"{RezervasyonControllerBaseUrl}/";
        public static string RezervasyonControllerDeleteUrl =>$"{RezervasyonControllerBaseUrl}/";

        // SiparisController URL'leri
        public static string SiparisControllerBaseUrl =>$"{ApiBaseUrl}/Siparis";
        public static string SiparisControllerGetUrl =>SiparisControllerBaseUrl;
        public static string SiparisControllerGetByIdUrl =>$"{SiparisControllerBaseUrl}/";
        public static string SiparisControllerCreateUrl =>SiparisControllerBaseUrl;
        public static string SiparisControllerUpdateUrl =>$"{SiparisControllerBaseUrl}/";
        public static string SiparisControllerDeleteUrl =>$"{SiparisControllerBaseUrl}/";

        // SiparisDetayController URL'leri
        public static string SiparisDetayControllerBaseUrl =>$"{ApiBaseUrl}/SiparisDetay";
        public static string SiparisDetayControllerGetUrl =>SiparisDetayControllerBaseUrl;
        public static string SiparisDetayControllerGetByIdUrl =>$"{SiparisDetayControllerBaseUrl}/";
        public static string SiparisDetayControllerCreateUrl =>SiparisDetayControllerBaseUrl;
        public static string SiparisDetayControllerUpdateUrl =>$"{SiparisDetayControllerBaseUrl}/";
        public static string SiparisDetayControllerDeleteUrl =>$"{SiparisDetayControllerBaseUrl}/";

        // StokController URL'leri
        public static string StokControllerBaseUrl =>$"{ApiBaseUrl}/Stok";
        public static string StokControllerGetUrl =>StokControllerBaseUrl;
        public static string StokControllerGetByIdUrl =>$"{StokControllerBaseUrl}/";
        public static string StokControllerCreateUrl =>StokControllerBaseUrl;
        public static string StokControllerUpdateUrl =>$"{StokControllerBaseUrl}/";
        public static string StokControllerDeleteUrl =>$"{StokControllerBaseUrl}/";

        // TedarikciController URL'leri
        public static string TedarikciControllerBaseUrl =>$"{ApiBaseUrl}/Tedarikci";
        public static string TedarikciControllerGetUrl =>TedarikciControllerBaseUrl;
        public static string TedarikciControllerGetByIdUrl =>$"{TedarikciControllerBaseUrl}/";
        public static string TedarikciControllerCreateUrl =>TedarikciControllerBaseUrl;
        public static string TedarikciControllerUpdateUrl =>$"{TedarikciControllerBaseUrl}/";
        public static string TedarikciControllerDeleteUrl =>$"{TedarikciControllerBaseUrl}/";

        // TedarikSiparisiController URL'leri
        public static string TedarikSiparisiControllerBaseUrl =>$"{ApiBaseUrl}/TedarikSiparisi";
        public static string TedarikSiparisiControllerGetUrl =>TedarikSiparisiControllerBaseUrl;
        public static string TedarikSiparisiControllerGetByIdUrl =>$"{TedarikSiparisiControllerBaseUrl}/";
        public static string TedarikSiparisiControllerCreateUrl =>TedarikSiparisiControllerBaseUrl;
        public static string TedarikSiparisiControllerUpdateUrl =>$"{TedarikSiparisiControllerBaseUrl}/";
        public static string TedarikSiparisiControllerDeleteUrl =>$"{TedarikSiparisiControllerBaseUrl}/";
    }
}
