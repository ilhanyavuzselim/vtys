using Domain.rezervasyon;
using Domain.masa;
using Domain.musteri;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Common.Requests.Rezervasyon;

namespace WebApi.Controllers.RezervasyonController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezervasyonController : ControllerBase
    {
        private readonly IRepository<Rezervasyon> _rezervasyonRepository;
        private readonly IRepository<Masa> _masaRepository;
        private readonly IRepository<Musteri> _musteriRepository;

        public RezervasyonController(
            IRepository<Rezervasyon> rezervasyonRepository,
            IRepository<Masa> masaRepository,
            IRepository<Musteri> musteriRepository)
        {
            _rezervasyonRepository = rezervasyonRepository;
            _masaRepository = masaRepository;
            _musteriRepository = musteriRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRezervasyonlar()
        {
            var rezervasyonlar = await _rezervasyonRepository.GetAllAsync(r=>r.Masa, r=>r.Musteri);
            return Ok(rezervasyonlar);  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRezervasyonById(Guid id)
        {
            try
            {
                var rezervasyon = await _rezervasyonRepository.GetByIdAsync(id, r=>r.Masa , r=>r.Musteri); 
                return Ok(rezervasyon); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRezervasyon([FromBody] CreateRezervasyonRequest rezervasyon)
        {
            if (rezervasyon == null)
            {
                return BadRequest("Rezervasyon verisi geçersiz");
            }

            var masa = await _masaRepository.GetByIdAsync(rezervasyon.MasaID);

            if (masa == null)
            {
                return NotFound("Masa ID'si uyuşmazlığı");
            }

            var musteri = await _musteriRepository.GetByIdAsync(rezervasyon.MusteriID);
            
            if (musteri == null)
            {
                return NotFound("Müşteri ID'si uyuşmazlığı");
            }

            Rezervasyon r = new Rezervasyon()
            {
                Masa = masa,
                Musteri = musteri,
                MasaID = rezervasyon.MasaID,
                MusteriID = rezervasyon.MusteriID,
                RezervasyonTarihi = rezervasyon.RezervasyonTarihi,
            };

            await _rezervasyonRepository.AddAsync(r); 
            return CreatedAtAction(nameof(GetRezervasyonById), new { id = r.Id }, rezervasyon); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRezervasyon(Guid id, [FromBody] UpdateRezervasyonRequest rezervasyon)
        {
            var existingRezervasyon = await _rezervasyonRepository.GetByIdAsync(id);

            if (existingRezervasyon == null)
            {
                return NotFound("Rezervasyon bulunamadı");
            }

            if(rezervasyon.MasaID != null)
            {
                var masa = await _masaRepository.GetByIdAsync(rezervasyon.MasaID.Value);
                if (masa == null)
                {
                    return NotFound("Masa bulunamadı");
                }
                existingRezervasyon.Masa = masa;
                existingRezervasyon.MasaID = masa.Id;
            }

            if(rezervasyon.MusteriID != null)
            {
                var musteri = await _musteriRepository.GetByIdAsync(rezervasyon.MusteriID.Value);
                if(musteri == null)
                {
                    return NotFound("Müşteri Bulunamadı");
                }
                existingRezervasyon.Musteri = musteri;
                existingRezervasyon.MusteriID = musteri.Id;
            }

            if(rezervasyon.RezervasyonTarihi != null)
            {
                existingRezervasyon.RezervasyonTarihi = rezervasyon.RezervasyonTarihi.Value;
            }

            await _rezervasyonRepository.UpdateAsync(existingRezervasyon);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRezervasyon(Guid id)
        {
            try
            {
                await _rezervasyonRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }
    }
}
