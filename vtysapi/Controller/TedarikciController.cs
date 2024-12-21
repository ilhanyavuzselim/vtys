using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.tedarikci;
using Common.Requests.Tedarikci;

namespace WebApi.Controllers.TedarikciController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TedarikciController : ControllerBase
    {
        private readonly IRepository<Tedarikci> _tedarikciRepository;

        public TedarikciController(IRepository<Tedarikci> tedarikciRepository)
        {
            _tedarikciRepository = tedarikciRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTedarikciler()
        {
            var tedarikciler = await _tedarikciRepository.GetAllAsync();
            return Ok(tedarikciler);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTedarikciById(Guid id)
        {
            try
            {
                var tedarikci = await _tedarikciRepository.GetByIdAsync(id);
                return Ok(tedarikci);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTedarikci([FromBody] CreateTedarikciRequest tedarikci)
        {
            if (tedarikci == null)
            {
                return BadRequest("Tedarikçi verisi geçersiz");
            }
            Tedarikci t = new Tedarikci()
            {
                Ad = tedarikci.Ad,
                Iletisim = tedarikci.Iletisim,
            };
            await _tedarikciRepository.AddAsync(t);
            return CreatedAtAction(nameof(GetTedarikciById), new { id = t.Id }, tedarikci);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTedarikci(Guid id, [FromBody] UpdateTedarikciRequest tedarikci)
        {
            var existedTedarikci = await _tedarikciRepository.GetByIdAsync(id);

            if (existedTedarikci == null)
            {
                return BadRequest("Tedarikçi ID'si uyuşmazlığı");
            }

            existedTedarikci.Ad = tedarikci.Ad;
            existedTedarikci.Iletisim = tedarikci.Iletisim;

            await _tedarikciRepository.UpdateAsync(existedTedarikci);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTedarikci(Guid id)
        {
            try
            {
                await _tedarikciRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
