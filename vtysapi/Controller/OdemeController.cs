using Microsoft.AspNetCore.Mvc;
using Domain.masa;
using Infrastructure.Repositories;
using Domain.odemeturu;
using Common.Requests.OdemeTuru;
using Domain.odeme;
using Common.Requests.Odeme;
using Domain.siparis;

namespace WebApi.Controllers.OdemeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class OdemeController : ControllerBase
    {
        private readonly IRepository<Odeme> _odemeRepository;
        private readonly IRepository<OdemeTuru> _odemeTuruRepository;
        private readonly IRepository<Siparis> _siparisRepository;

        public OdemeController(IRepository<Odeme> odemeRepository, IRepository<Siparis> siparisRepository, IRepository<OdemeTuru> odemeTuruRepository)
        {
            _odemeRepository = odemeRepository;
            _siparisRepository = siparisRepository;
            _odemeTuruRepository = odemeTuruRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOdeme()
        {
            var odeme = await _odemeRepository.GetAllAsync(o => o.Siparis, od=>od.OdemeTuru);
            return Ok(odeme);  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOdemeById(Guid id)
        {
            try
            {
                var odeme = await _odemeRepository.GetByIdAsync(id, o => o.Siparis, od => od.OdemeTuru); 
                return Ok(odeme); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOdeme([FromBody] CreateOdemeRequest odeme)
        {
            if (odeme == null)
            {
                return BadRequest("Masa verisi geçersiz");
            }

            var siparis = await _siparisRepository.GetByIdAsync(odeme.SiparisID);

            if(siparis == null)
            {
                return BadRequest("Sipariş ID'si uyuşmazlığı");
            }

            var odemeTuru = await _odemeTuruRepository.GetByIdAsync(odeme.OdemeTuruID);

            if(odemeTuru == null)
            {
                return BadRequest("Odeme türü ID'si uyuşmazlığı");
            }

            Odeme o = new Odeme()
            {
                OdemeTuruID = odeme.OdemeTuruID,
                SiparisID = odeme.SiparisID,
                Tutar = odeme.Tutar,
            };

            await _odemeRepository.AddAsync(o); 
            return CreatedAtAction(nameof(GetOdemeById), new { id = o.Id } , odeme); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOdeme(Guid id, [FromBody] UpdateOdemeRequest odeme)
        {
            var updatedOdeme = await _odemeRepository.GetByIdAsync(id);
            
            if (updatedOdeme == null) 
            {
                return BadRequest("Masa ID'si uyuşmazlığı");
            }

            var siparis = await _siparisRepository.GetByIdAsync(odeme.SiparisID);

            if (siparis == null)
            {
                return BadRequest("Sipariş ID'si uyuşmazlığı");
            }

            var odemeTuru = await _odemeTuruRepository.GetByIdAsync(odeme.OdemeTuruID);

            if (odemeTuru == null)
            {
                return BadRequest("Odeme türü ID'si uyuşmazlığı");
            }

            updatedOdeme.Tutar = odeme.Tutar;
            updatedOdeme.OdemeTuruID = odeme.OdemeTuruID;
            updatedOdeme.OdemeTuru = odemeTuru;
            updatedOdeme.SiparisID = siparis.Id;
            updatedOdeme.Siparis = siparis;

            await _odemeRepository.UpdateAsync(updatedOdeme); 
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOdeme(Guid id)
        {
            try
            {
                await _odemeRepository.DeleteAsync(id); 
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }
    }
}
