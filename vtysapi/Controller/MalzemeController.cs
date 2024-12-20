using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.malzeme;
using Common.Requests.Malzeme;
using Domain.tedarikci;

namespace WebApi.Controllers.MalzemeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class MalzemeController : ControllerBase
    {
        private readonly IRepository<Malzeme> _malzemeRepository;
        private readonly IRepository<Tedarikci> _tedarikciRepository;

        public MalzemeController(IRepository<Malzeme> malzemeRepository, IRepository<Tedarikci> tedarikciRepository)
        {
            _malzemeRepository = malzemeRepository;
            _tedarikciRepository = tedarikciRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMalzemeler()
        {
            var malzemeler = await _malzemeRepository.GetAllAsync(m=> m.Tedarikci);
            return Ok(malzemeler);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMalzemeById(Guid id)
        {
            try
            {
                var malzeme = await _malzemeRepository.GetByIdAsync(id, m => m.Tedarikci);
                return Ok(malzeme);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMalzeme([FromBody] CreateMalzemeRequest malzeme)
        {
            if (malzeme == null)
            {
                return BadRequest("Masa verisi geçersiz");
            }

            var tedarikci = await _tedarikciRepository.GetByIdAsync(malzeme.TedarikciID);

            if(tedarikci == null)
            {
                return BadRequest("Tedarikci ID'si uyuşmazlığı");
            }

            Malzeme m = new Malzeme()
            {
                Ad=malzeme.Ad,
                TedarikciID=malzeme.TedarikciID,  
            };
            await _malzemeRepository.AddAsync(m);
            return CreatedAtAction(nameof(GetMalzemeById), new { id = m.Id }, malzeme);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMalzeme(Guid id, [FromBody] UpdataMalzemeRequest malzeme)
        {
           

            var updatedMalzeme = await _malzemeRepository.GetByIdAsync(id);

            if (updatedMalzeme == null)
            {
                return BadRequest("Masa ID'si uyuşmazlığı");
            }
            
            var newTedarikci = await _tedarikciRepository.GetByIdAsync(malzeme.TedarikciID);

            if(newTedarikci == null)
            {
                return BadRequest("Tedarikci ID'si uyuşmazlığı");
            }

            updatedMalzeme.Ad = malzeme.Ad;
            updatedMalzeme.TedarikciID = malzeme.TedarikciID;
            updatedMalzeme.Tedarikci = newTedarikci;
            

            await _malzemeRepository.UpdateAsync(updatedMalzeme);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMalzeme(Guid id)
        {
            try
            {
                await _malzemeRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
