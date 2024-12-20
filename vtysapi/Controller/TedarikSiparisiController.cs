using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.odeme;
using Common.Requests.Odeme;
using Domain.tedarikci;
using Domain.malzeme;
using Domain.tedariksiparisi;
using Common.Requests.TedarikSiparisi;

namespace WebApi.Controllers.OdemeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TedarikSiparisiController : ControllerBase
    {
        private readonly IRepository<TedarikSiparisi> _tedarikciSiparisiRepository;
        private readonly IRepository<Tedarikci> _tedarikciRepository;
        private readonly IRepository<Malzeme> _malzemeRepository;

        public TedarikSiparisiController(IRepository<Malzeme> malzemeRepository, IRepository<Tedarikci> tedarikciRepository, IRepository<TedarikSiparisi> tedarikciSiparisiRepository)
        {
            _malzemeRepository = malzemeRepository;
            _tedarikciRepository = tedarikciRepository;
            _tedarikciSiparisiRepository = tedarikciSiparisiRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTedarikSiparisi()
        {
            var tedarikSiparisleri = await _tedarikciSiparisiRepository.GetAllAsync(t => t.Tedarikci, te=>te.Malzeme);
            return Ok(tedarikSiparisleri);  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTedarikSiparisiById(Guid id)
        {
            try
            {
                var tedarikSiparisi = await _tedarikciSiparisiRepository.GetByIdAsync(id, t => t.Tedarikci, te => te.Malzeme); 
                return Ok(tedarikSiparisi); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTedarikSiparisi([FromBody] CreaetTedarikSiparisiRequest tedarikSiparisi)
        {
            if (tedarikSiparisi == null)
            {
                return BadRequest("Tedarik Siparişi verisi geçersiz");
            }

            var tedarikci = await _tedarikciRepository.GetByIdAsync(tedarikSiparisi.TedarikciID);

            if(tedarikci == null)
            {
                return BadRequest("Tedarikçi ID'si uyuşmazlığı");
            }

            var malzeme = await _malzemeRepository.GetByIdAsync(tedarikSiparisi.MalzemeID);

            if(malzeme == null)
            {
                return BadRequest("Malzeme ID'si uyuşmazlığı");
            }

            TedarikSiparisi t = new TedarikSiparisi()
            {
                Malzeme = malzeme,
                MalzemeID = tedarikSiparisi.MalzemeID,
                Tedarikci = tedarikci,
                TedarikciID = tedarikSiparisi.TedarikciID,
                Miktar = tedarikSiparisi.Miktar,
                SiparisTarihi = tedarikSiparisi.SiparisTarihi,
                BirimFiyat = tedarikSiparisi.BirimFiyat,
            };

            await _tedarikciSiparisiRepository.AddAsync(t); 
            return CreatedAtAction(nameof(GetTedarikSiparisiById), new { id = t.Id } , tedarikSiparisi); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTedarikSiparisi(Guid id, [FromBody] UpdateTedarikSiparisiRequest tedarikSiparisi)
        {
            if (tedarikSiparisi == null)
            {
                return BadRequest("Tedarik Siparişi verisi geçersiz");
            }

            var updatedTedarikSiparisi = await _tedarikciSiparisiRepository.GetByIdAsync(id);

            if (updatedTedarikSiparisi == null)
            {
                return BadRequest("Tedarik Siparişi ID'si uyuşmazlığı");
            }

            var existedTedarikci = await _tedarikciRepository.GetByIdAsync(tedarikSiparisi.TedarikciID);

            if (existedTedarikci == null)
            {
                return BadRequest("Tedarikçi ID'si uyuşmazlığı");
            }

            var existedMalzeme = await _malzemeRepository.GetByIdAsync(tedarikSiparisi.MalzemeID);

            if (existedMalzeme == null)
            {
                return BadRequest("Malzeme ID'si uyuşmazlığı");
            }

            updatedTedarikSiparisi.BirimFiyat = tedarikSiparisi.BirimFiyat;
            updatedTedarikSiparisi.Miktar = tedarikSiparisi.Miktar;
            updatedTedarikSiparisi.SiparisTarihi = tedarikSiparisi.SiparisTarihi;
            updatedTedarikSiparisi.Malzeme = existedMalzeme;
            updatedTedarikSiparisi.MalzemeID = tedarikSiparisi.MalzemeID;
            updatedTedarikSiparisi.Tedarikci = existedTedarikci;
            updatedTedarikSiparisi.TedarikciID = tedarikSiparisi.TedarikciID;

            await _tedarikciSiparisiRepository.UpdateAsync(updatedTedarikSiparisi); 
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTedarikSiparisi(Guid id)
        {
            try
            {
                await _tedarikciSiparisiRepository.DeleteAsync(id); 
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }
    }
}
