using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.siparis;
using Domain.masa;
using Domain.musteri;
using Domain.personel;
using Common.Requests.Siparis;
using Domain.kisi;
using Domain.siparisdetay;
using System.Linq.Expressions;
using Domain.menu;

namespace WebApi.Controllers.MenuController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiparisController : ControllerBase
    {
        private readonly IRepository<Siparis> _siparisRepository;
        private readonly IRepository<Masa> _masaRepository;
        private readonly IRepository<Kisi> _kisiRepository;
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<SiparisDetay> _siparisDetayRepository;

        public SiparisController(IRepository<Siparis> siparisRepository, IRepository<Masa> masaRepository,
            IRepository<Kisi> kisiRepository, IRepository<Menu> menuRepository, IRepository<SiparisDetay> siparisDetayRepository)
        {
            _siparisRepository = siparisRepository;
            _masaRepository = masaRepository;
            _kisiRepository = kisiRepository;
            _menuRepository = menuRepository;
            _siparisDetayRepository = siparisDetayRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSiparisler()
        {
            var siparisler = await _siparisRepository.GetAllAsync(s => s.Masa , si => si.SiparisDetaylar);
            return Ok(siparisler);  
        }

        [HttpGet("GetSiparisByMasaId/{id}")]
        public async Task<IActionResult> GetSiparisByMasaId(Guid id)
        {
            var siparis = await _siparisRepository.GetAllByPredicatesAndIncludes([s => s.OdendiMi == false, s=>s.MasaID == id, s=>s.SiparisDetaylar.Any(sd=>sd.OdendiMi == false)], [s => s.SiparisDetaylar.Where(sd=>sd.OdendiMi == false)]);
            var menuler = await _menuRepository.GetAllAsync();
            
            foreach(var siparisDetay in siparis.First().SiparisDetaylar)
            {
                siparisDetay.Menu = menuler.FirstOrDefault(m => m.Id == siparisDetay.MenuID);    
            }

            if (!siparis.Any())
            {
                return NotFound("Ödenmemiş Sipariş Bulunamadı");
            }
            return Ok(siparis.First());
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

            var musteri = await _kisiRepository.GetByIdAsync(siparis.MusteriID);

            if (musteri == null)
            {
                return BadRequest("Verilen Müşteri Bulunamadı");
            }

            var personel = await _kisiRepository.GetByIdAsync(siparis.PersonelID);
            
            if (personel == null)
            {
                return BadRequest("Verilen personel Peronsel Bulunamadı");
            }

            masa.Durum = false;

            Siparis s = new Siparis()
            {
                Id = Guid.NewGuid(),
                Masa = masa,
                PersonelID = siparis.PersonelID,
                MusteriID = siparis.MusteriID,
                MasaID = siparis.MasaID,
                SiparisTarihi = siparis.SiparisTarihi.ToUniversalTime(),
                SiparisDetaylar = new List<SiparisDetay>()
            };

            if (siparis.SiparisDetaylar.Any())
            {
                foreach (var createSiparisDetayRequest in siparis.SiparisDetaylar) 
                {
                    var siparisDetay = new SiparisDetay()
                    {
                        Adet = createSiparisDetayRequest.Adet,
                        MenuID = createSiparisDetayRequest.MenuID,
                        SiparisID = createSiparisDetayRequest.SiparisID
                    };
                    s.SiparisDetaylar.Add(siparisDetay);
                }
            }

            await _siparisRepository.AddAsync(s); 
            await _masaRepository.UpdateAsync(masa);
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

            if (siparis.MusteriID.HasValue)
            {
                var musteri = await _kisiRepository.GetByIdAsync(siparis.MusteriID.Value);
                if (musteri == null)
                {
                    return BadRequest("Müşteri ID'si uyuşmazlığı");
                }
            }

            if (siparis.PersonelID.HasValue)
            {
                var personel = await _kisiRepository.GetByIdAsync(siparis.PersonelID.Value);
                if (personel == null)
                {
                    return BadRequest("Personel ID'si uyuşmazlığı");
                }
            }

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

        [HttpPost("CompleteSiparisDetayListByIdList")]
        public async Task<IActionResult> CompleteSiparisDetayListByIdList([FromBody] List<Guid> siparisDetayIdList)
        {
            var siparisDetayları = await _siparisDetayRepository.GetAllByPredicate(sd => sd.OdendiMi == false);
            var siparisId = siparisDetayları.FirstOrDefault(sd => sd.Id == siparisDetayIdList.First()).SiparisID;
            foreach(var id in siparisDetayIdList)
            {
                var completedSiparisDetayi = siparisDetayları.FirstOrDefault(sd => sd.Id == id);
                completedSiparisDetayi.OdendiMi = true;
                await _siparisDetayRepository.UpdateAsync(completedSiparisDetayi);
            }
            var siparis = await _siparisRepository.GetByIdAsync(siparisId,s => s.SiparisDetaylar, siparis=>siparis.Masa);
            if(!siparis.SiparisDetaylar.Any(sd => sd.OdendiMi == false))
            {
                siparis.OdendiMi = true;
                siparis.Masa.Durum = true;
                await _siparisRepository.UpdateAsync(siparis);
            }
            return NoContent();
        }
    }
}
