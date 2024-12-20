using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Common.Requests.Tedarikci;
using Domain.stok;
using Common.Requests.Stok;
using Domain.malzeme;

namespace WebApi.Controllers.StokController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StokController : ControllerBase
    {
        private readonly IRepository<Stok> _stokRepository;
        private readonly IRepository<Malzeme> _malzemeRepository;

        public StokController(IRepository<Stok> stokRepositor, IRepository<Malzeme> malzemeRepository)
        {
            _stokRepository = stokRepositor;
            _malzemeRepository = malzemeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStoklar()
        {
            var stoklar = await _stokRepository.GetAllAsync(s => s.Malzeme);
            return Ok(stoklar);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStokById(Guid id)
        {
            try
            {
                var tedarikci = await _stokRepository.GetByIdAsync(id, s=>s.Malzeme);
                return Ok(tedarikci);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTedarikci([FromBody] CreateStokRequest stok)
        {
            if (stok == null)
            {
                return BadRequest("Stok verisi geçersiz");
            }

            var malzeme = await _malzemeRepository.GetByIdAsync(stok.MalzemeID);

            if (malzeme == null)
            {
                return BadRequest("Malzeme verisi geçersiz");
            }

            Stok s = new Stok()
            {
                MalzemeID = stok.MalzemeID,
                Malzeme = malzeme,
                Miktar = stok.Miktar,
            };
            await _stokRepository.AddAsync(s);
            return CreatedAtAction(nameof(GetStokById), new { id = s.Id }, stok);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStok(Guid id, [FromBody] UpdateStokRequest stok)
        {
            var existedStok = await _stokRepository.GetByIdAsync(id);

            if (existedStok == null)
            {
                return BadRequest("Kategori ID'si uyuşmazlığı");
            }

            var existedMalzeme = await _malzemeRepository.GetByIdAsync(stok.MalzemeID);

            if (existedMalzeme == null)
            {
                return BadRequest("Kategori ID'si uyuşmazlığı");
            }

            existedStok.Miktar = stok.Miktar;
            existedStok.MalzemeID = stok.MalzemeID;
            existedStok.Malzeme = existedMalzeme;

            await _stokRepository.UpdateAsync(existedStok);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStok(Guid id)
        {
            try
            {
                await _stokRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
