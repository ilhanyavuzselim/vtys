using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.odemeturu;
using Common.Requests.OdemeTuru;

namespace WebApi.Controllers.OdemeTuruController
{
    [Route("api/[controller]")]
    [ApiController]
    public class OdemeTuruController : ControllerBase
    {
        private readonly IRepository<OdemeTuru> _odemeTuruRepository;

        public OdemeTuruController(IRepository<OdemeTuru> odemeTuruRepository)
        {
            _odemeTuruRepository = odemeTuruRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOdemeTurleri()
        {
            var odemeTurleri = await _odemeTuruRepository.GetAllAsync();
            return Ok(odemeTurleri);  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOdemeTuruById(Guid id)
        {
            try
            {
                var odemeTuru = await _odemeTuruRepository.GetByIdAsync(id); 
                return Ok(odemeTuru); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOdemeTuru([FromBody] CreateOdemeTuruRequest odemeTuru)
        {
            if (odemeTuru == null)
            {
                return BadRequest("Ödeme Türü verisi geçersiz");
            }
            OdemeTuru o = new OdemeTuru()
            {
                Ad = odemeTuru.Ad,
            };
            await _odemeTuruRepository.AddAsync(o); 
            return CreatedAtAction(nameof(GetOdemeTuruById), new { id = o.Id } , odemeTuru); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOdemeTuru(Guid id, [FromBody] UpdateOdemeTuruRequest odemeTuru)
        {
            var updatedOdemeTuru = await _odemeTuruRepository.GetByIdAsync(id);
            
            if (updatedOdemeTuru == null) 
            {
                return BadRequest("Ödeme Türü ID'si uyuşmazlığı");
            }
            
            updatedOdemeTuru.Ad = odemeTuru.Ad;

            await _odemeTuruRepository.UpdateAsync(updatedOdemeTuru); 
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOdemeTuru(Guid id)
        {
            try
            {
                await _odemeTuruRepository.DeleteAsync(id); 
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }
    }
}
