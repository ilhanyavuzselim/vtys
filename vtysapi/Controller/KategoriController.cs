using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.kategori;
using Common.Requests.Kategori;

namespace WebApi.Controllers.MenuController
{
    [Route("api/[controller]")]
    [ApiController]
    public class KategoriController : ControllerBase
    {
        private readonly IRepository<Kategori> _kategoriRepository;

        public KategoriController(IRepository<Kategori> kategoriRepository)
        {
            _kategoriRepository = kategoriRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllKategoriler()
        {
            var kategoriler = await _kategoriRepository.GetAllAsync(k => k.Menuler);
            return Ok(kategoriler);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetKategoriById(Guid id)
        {
            try
            {
                var kategori = await _kategoriRepository.GetByIdAsync(id, k => k.Menuler);
                return Ok(kategori);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateKategori([FromBody] CreateKategoriRequest kategori)
        {
            if (kategori == null)
            {
                return BadRequest("Kategori verisi geçersiz");
            }
            Kategori k = new Kategori()
            {
                Ad = kategori.Ad,
            };
            await _kategoriRepository.AddAsync(k);
            return CreatedAtAction(nameof(GetKategoriById), new { id = k.Id }, kategori);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKategori(Guid id, [FromBody] UpdateKategoriRequest kategori)
        {
            var existedKategori = await _kategoriRepository.GetByIdAsync(id);

            if (existedKategori == null)
            {
                return BadRequest("Kategori ID'si uyuşmazlığı");
            }

            existedKategori.Ad = kategori.Ad;

            await _kategoriRepository.UpdateAsync(existedKategori);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKategori(Guid id)
        {
            try
            {
                await _kategoriRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
