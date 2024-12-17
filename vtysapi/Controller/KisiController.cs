using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.kisi;
using Common.Requests.kisi;

namespace WebApi.Controllers.KisiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class KisiController : ControllerBase
    {
        private readonly IRepository<Kisi> _kisiRepository;

        public KisiController(IRepository<Kisi> kisiRepository)
        {
            _kisiRepository = kisiRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllKisiler()
        {
            var kisiler = await _kisiRepository.GetAllAsync();
            return Ok(kisiler);  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetKisiById(Guid id)
        {
            try
            {
                var kisi = await _kisiRepository.GetByIdAsync(id); 
                return Ok(kisi); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateKisi([FromBody] CreateKisiRequest kisi)
        {
            if (kisi == null)
            {
                return BadRequest("Kişi verisi geçersiz");
            }
            Kisi k = new Kisi(Ad: kisi.Ad, Soyad: kisi.Soyad);
            await _kisiRepository.AddAsync(k); 
            return CreatedAtAction(nameof(GetKisiById), new {id = k.Id} , kisi); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKisi(Guid id, [FromBody] UpdateKisiRequest kisi)
        {
            var updatedKisi = await _kisiRepository.GetByIdAsync(id);
            
            if (updatedKisi == null) 
            {
                return BadRequest("Kişi ID'si uyuşmazlığı");
            }
            
            updatedKisi.Ad = kisi.Ad == default ? updatedKisi.Ad : kisi.Ad;
            updatedKisi.Soyad = kisi.Soyad == default ? updatedKisi.Soyad : kisi.Soyad;

            await _kisiRepository.UpdateAsync(updatedKisi);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKisi(Guid id)
        {
            try
            {
                await _kisiRepository.DeleteAsync(id); 
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
