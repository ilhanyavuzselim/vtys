using Microsoft.AspNetCore.Mvc;
using Domain.masa;
using Infrastructure.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasaController : ControllerBase
    {
        private readonly IRepository<Masa> _masaRepository;

        // Constructor Injection ile IRepository<Masalar> referansı alınır
        public MasaController(IRepository<Masa> masaRepository)
        {
            _masaRepository = masaRepository;
        }

        // Tüm masaları getiren API endpoint'i
        [HttpGet]
        public async Task<IActionResult> GetAllMasalar()
        {
            var masalar = await _masaRepository.GetAllAsync();
            return Ok(masalar);  // Masalar listesi döndürülür
        }

        // ID ile masa getiren API endpoint'i
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMasaById(int id)
        {
            try
            {
                var masa = await _masaRepository.GetByIdAsync(id); // ID'ye göre masa getirilir
                return Ok(masa); // Masa bulunursa, OK döndürülür
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Masa bulunamazsa, 404 NotFound döndürülür
            }
        }

        // Yeni masa ekleyen API endpoint'i
        [HttpPost]
        public async Task<IActionResult> CreateMasa([FromBody] Masa masa)
        {
            if (masa == null)
            {
                return BadRequest("Masa verisi geçersiz");
            }

            await _masaRepository.AddAsync(masa); // Yeni masa eklenir
            return CreatedAtAction(nameof(GetMasaById), new { id = masa.MasaID }, masa); // Ekleme başarılı ise 201 döndürülür
        }

        // Mevcut bir masayı güncelleyen API endpoint'i
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMasa(Guid id, [FromBody] Masa masa)
        {
            if (id != masa.MasaID) // Eğer ID'ler eşleşmezse BadRequest döndürülür
            {
                return BadRequest("Masa ID'si uyuşmazlığı");
            }

            await _masaRepository.UpdateAsync(masa); // Masa güncellenir
            return NoContent(); // Güncelleme başarılıysa 204 döndürülür
        }

        // Bir masayı silen API endpoint'i
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasa(int id)
        {
            try
            {
                await _masaRepository.DeleteAsync(id); // ID'ye göre masa silinir
                return NoContent(); // Silme başarılıysa 204 döndürülür
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Eğer masa bulunmazsa 404 döndürülür
            }
        }
    }
}
