using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.menu;
using Common.Requests.Menu;
using Domain.siparis;
using Domain.masa;
using Domain.musteri;
using Domain.personel;
using Common.Requests.Siparis;

namespace WebApi.Controllers.MenuController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiparisController : ControllerBase
    {
        private readonly IRepository<Siparis> _siparisRepository;
        private readonly IRepository<Masa> _masaRepository;
        private readonly IRepository<Musteri> _musteriRepository;
        private readonly IRepository<Personel> _personelRepository;

        public SiparisController(IRepository<Siparis> siparisRepository, IRepository<Masa> masaRepository, IRepository<Musteri> musteriRepository, IRepository<Personel> personelRepository)
        {
            _siparisRepository = siparisRepository;
            _masaRepository = masaRepository;
            _musteriRepository = musteriRepository;
            _personelRepository = personelRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSiparisler()
        {
            var siparisler = await _siparisRepository.GetAllAsync(s => s.Masa , si => si.SiparisDetaylar);
            return Ok(siparisler);  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSiparisById(Guid id)
        {
            try
            {
                var siparis = await _siparisRepository.GetByIdAsync(id, s => s.Masa, si => si.SiparisDetaylar);
                return Ok(siparis);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSiparis([FromBody] CreateSiparisRequest siparis)
        {
            if (siparis == null)
            {
                return BadRequest("Sipariş verisi geçersiz");
            }
            
            var masa = await _masaRepository.GetByIdAsync(siparis.MasaID);

            if(masa == null)
            {
                return BadRequest("Verilen Masa Bulunamadı");
            }

            /*var musteri = await _musteriRepository.GetByIdAsync(siparis.MusteriID);

            if (musteri == null)
            {
                return BadRequest("Verilen Müşteri Bulunamadı");
            }

            var personel = await _personelRepository.GetByIdAsync(siparis.PersonelID);

            if (personel == null)
            {
                return BadRequest("Verilen personel Peronsel Bulunamadı");
            }*/

            Siparis s = new Siparis()
            {
                Masa = masa,
                PersonelID = siparis.PersonelID,
                MusteriID = siparis.MusteriID,
                MasaID = siparis.MasaID,
                SiparisTarihi = siparis.SiparisTarihi
            };

            await _siparisRepository.AddAsync(s); 
            return CreatedAtAction(nameof(GetSiparisById), new { id = s.Id }, siparis);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSiparis(Guid id, [FromBody] UpdateSiparisRequest siparis)
        {
            var existedSiparis = await _siparisRepository.GetByIdAsync(id);

            if (existedSiparis == null) 
            {
                return BadRequest("Sipariş ID'si uyuşmazlığı");
            }

            if (siparis.MasaID.HasValue)
            {
                var masa = await _masaRepository.GetByIdAsync(siparis.MasaID.Value);
                if(masa == null)
                {
                    return BadRequest("Masa ID'si uyuşmazlığı");
                }
                existedSiparis.Masa = masa;
            }

            /*
             * if (siparis.MusteriID.HasValue)
            {
                var musteri = await _musteriRepository.GetByIdAsync(siparis.MusteriID.Value);
                if (musteri == null)
                {
                    return BadRequest("Müşteri ID'si uyuşmazlığı");
                }
                existedSiparis.Musteri = musteri;
            }

            if (siparis.PersonelID.HasValue)
            {
                var personel = await _personelRepository.GetByIdAsync(siparis.PersonelID.Value);
                if (personel == null)
                {
                    return BadRequest("Personel ID'si uyuşmazlığı");
                }
                existedSiparis.Personel = personel;
            }
            */

            existedSiparis.PersonelID = siparis.PersonelID == null ? existedSiparis.PersonelID : siparis.PersonelID.Value;
            existedSiparis.MasaID = siparis.MasaID == null ? existedSiparis.MasaID : siparis.MasaID.Value;
            existedSiparis.MusteriID = siparis.MusteriID == null ? existedSiparis.MusteriID : siparis.MusteriID.Value;
            existedSiparis.SiparisTarihi = siparis.SiparisTarihi.HasValue ? siparis.SiparisTarihi.Value : existedSiparis.SiparisTarihi;

            await _siparisRepository.UpdateAsync(existedSiparis); 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSiparis(Guid id)
        {
            try
            {
                await _siparisRepository.DeleteAsync(id); 
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }
    }
}
