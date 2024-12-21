using Microsoft.AspNetCore.Mvc;
using Domain.masa;
using Infrastructure.Repositories;
using Common.Requests.Masa;

namespace WebApi.Controllers.MasaController
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasaController : ControllerBase
    {
        private readonly IRepository<Masa> _masaRepository;

        public MasaController(IRepository<Masa> masaRepository)
        {
            _masaRepository = masaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMasalar()
        {
            var masalar = await _masaRepository.GetAllAsync();
            return Ok(masalar);  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMasaById(Guid id)
        {
            try
            {
                var masa = await _masaRepository.GetByIdAsync(id); 
                return Ok(masa); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMasa([FromBody] CreateMasaRequest masa)
        {
            if (masa == null)
            {
                return BadRequest("Masa verisi geçersiz");
            }
            var existedMasa = await _masaRepository.GetAllByPredicate(m => m.MasaNo == masa.MasaNo);

            if(existedMasa != null)
            {
                return BadRequest("Verilen Masa No eşsiz olmalıdır");
            }

            Masa m = new Masa()
            {
                Kapasite = masa.Kapasite,
                MasaNo = masa.MasaNo,
                Durum = masa.Durum
            };
            await _masaRepository.AddAsync(m); 
            return CreatedAtAction(nameof(GetMasaById), new { id = m.Id } , masa); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMasa(Guid id, [FromBody] UpdateMasaRequest masa)
        {
            if (id != masa.Id)
            {
                return BadRequest("Masa ID'si ile URL uyuşmazlığı");
            }

            var updatedMasa = await _masaRepository.GetByIdAsync(id);
            
            if (updatedMasa == null) 
            {
                return BadRequest("Masa ID'si uyuşmazlığı");
            }
            
            updatedMasa.MasaNo = masa.MasaNo;
            updatedMasa.Kapasite = masa.Kapasite;
            updatedMasa.Durum = masa.Durum;

            await _masaRepository.UpdateAsync(updatedMasa); 
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasa(Guid id)
        {
            try
            {
                await _masaRepository.DeleteAsync(id); 
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }
    }
}
