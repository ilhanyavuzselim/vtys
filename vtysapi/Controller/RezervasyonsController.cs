using Domain.rezervasyon;
using Domain.masa;
using Domain.musteri;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using vtysapi.Requests;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezervasyonController : ControllerBase
    {
        private readonly IRepository<Rezervasyon> _rezervasyonRepository;
        private readonly IRepository<Masa> _masaRepository;
        private readonly IRepository<Musteri> _musteriRepository;

        // Constructor Injection ile IRepository<Rezervasyon>, IRepository<Masa>, IRepository<Musteri> referansları alınır
        public RezervasyonController(
            IRepository<Rezervasyon> rezervasyonRepository,
            IRepository<Masa> masaRepository,
            IRepository<Musteri> musteriRepository)
        {
            _rezervasyonRepository = rezervasyonRepository;
            _masaRepository = masaRepository;
            _musteriRepository = musteriRepository;
        }

        // Tüm rezervasyonları getiren API endpoint'i
        [HttpGet]
        public async Task<IActionResult> GetAllRezervasyonlar()
        {
            var rezervasyonlar = await _rezervasyonRepository.GetAllAsync(r=>r.Masa, r=>r.Musteri);
            return Ok(rezervasyonlar);  // Rezervasyonlar listesi döndürülür
        }

        // ID ile rezervasyon getiren API endpoint'i
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRezervasyonById(Guid id)
        {
            try
            {
                var rezervasyon = await _rezervasyonRepository.GetByIdAsync(id, r=>r.Masa , r=>r.Musteri); // ID'ye göre rezervasyon getirilir
                return Ok(rezervasyon); // Rezervasyon bulunursa, OK döndürülür
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Rezervasyon bulunamazsa, 404 NotFound döndürülür
            }
        }

        // Yeni rezervasyon ekleyen API endpoint'i
        [HttpPost]
        public async Task<IActionResult> CreateRezervasyon([FromBody] CreateRezervasyonRequest rezervasyon)
        {
            if (rezervasyon == null)
            {
                return BadRequest("Rezervasyon verisi geçersiz");
            }

            // Masa ve Müşteri kontrolü
            var masa = await _masaRepository.GetByIdAsync(rezervasyon.MasaID);
            var musteri = await _musteriRepository.GetByIdAsync(rezervasyon.MusteriID);

            if (masa == null || musteri == null)
            {
                return NotFound("Masa veya Müşteri bulunamadı");
            }

            Rezervasyon r = new Rezervasyon(rezervasyon.MasaID, rezervasyon.MusteriID, rezervasyon.RezervasyonTarihi);
            r.Musteri = await _musteriRepository.GetByIdAsync(rezervasyon.MusteriID);
            r.Masa = await _masaRepository.GetByIdAsync(rezervasyon.MasaID);

            await _rezervasyonRepository.AddAsync(r); // Yeni rezervasyon eklenir
            return CreatedAtAction(nameof(GetRezervasyonById), new { id = r.Id }, rezervasyon); // Ekleme başarılı ise 201 döndürülür
        }

        // Mevcut bir rezervasyonu güncelleyen API endpoint'i
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRezervasyon(Guid id, [FromBody] UpdateRezervasyonRequest rezervasyon)
        {
            if (id != rezervasyon.Id) // Eğer ID'ler eşleşmezse BadRequest döndürülür
            {
                return BadRequest("Rezervasyon ID'si uyuşmazlığı");
            }

            // Masa ve Müşteri kontrolü
            var masa = await _masaRepository.GetByIdAsync(rezervasyon.MasaID);
            var musteri = await _musteriRepository.GetByIdAsync(rezervasyon.MusteriID);

            if (masa == null || musteri == null)
            {
                return NotFound("Masa veya Müşteri bulunamadı");
            }

            // Veritabanından mevcut rezervasyonu al
            var existingRezervasyon = await _rezervasyonRepository.GetByIdAsync(id);

            if (existingRezervasyon == null)
            {
                return NotFound("Rezervasyon bulunamadı");
            }

            // Rezervasyonu güncelleme işlemi
            existingRezervasyon.Masa = masa;
            existingRezervasyon.Musteri = musteri;
            existingRezervasyon.MasaID = rezervasyon.MasaID;
            existingRezervasyon.MusteriID = rezervasyon.MusteriID;
            existingRezervasyon.RezervasyonTarihi = rezervasyon.RezervasyonTarihi;

            // `DbContext`'te takip edilen nesneyi güncelle
            await _rezervasyonRepository.UpdateAsync(existingRezervasyon);

            return NoContent(); // Güncelleme başarılıysa 204 döndürülür
        }


        // Bir rezervasyonu silen API endpoint'i
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRezervasyon(Guid id)
        {
            try
            {
                await _rezervasyonRepository.DeleteAsync(id); // ID'ye göre rezervasyon silinir
                return NoContent(); // Silme başarılıysa 204 döndürülür
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Eğer rezervasyon bulunmazsa 404 döndürülür
            }
        }
    }
}
