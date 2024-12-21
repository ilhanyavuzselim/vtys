using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.menu;
using Domain.siparisdetay;
using Domain.siparis;
using Common.Requests.SiparisDetay;

namespace WebApi.Controllers.MenuController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiparisDetayController : ControllerBase
    {
        private readonly IRepository<SiparisDetay> _siparisDetayRepository;
        private readonly IRepository<Siparis> _siparisRepository;
        private readonly IRepository<Menu> _menuRepository;

        public SiparisDetayController(IRepository<Menu> menuRepository, IRepository<Siparis> siparisRepository, IRepository<SiparisDetay> siparisDetayRepository)
        {
            _menuRepository = menuRepository;
            _siparisRepository = siparisRepository;
            _siparisDetayRepository = siparisDetayRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSiparisDetaylar()
        {
            var siparisDetaylar = await _siparisDetayRepository.GetAllAsync(s => s.Siparis, si => si.Menu);
            return Ok(siparisDetaylar);  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSiparisDetayById(Guid id)
        {
            try
            {
                var siparisDetay = await _siparisDetayRepository.GetByIdAsync(id, s => s.Siparis, si => si.Menu);
                return Ok(siparisDetay);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSiparisDetay([FromBody] CreateSiparisDetayRequest siparisDetay)
        {
            if (siparisDetay == null)
            {
                return BadRequest("Sipariş Detay verisi geçersiz");
            }
            
            var existedMenu = await _menuRepository.GetByIdAsync(siparisDetay.MenuID);
            
            if(existedMenu == null)
            {
                return BadRequest("Verilen Menü Bulunamadı");
            }

            var existedSiparis = await _siparisRepository.GetByIdAsync(siparisDetay.SiparisID);

            if (existedSiparis == null)
            {
                return BadRequest("Verilen Sipariş Bulunamadı");
            }

            SiparisDetay s = new SiparisDetay()
            {
                Adet = siparisDetay.Adet,
                SiparisID = siparisDetay.SiparisID,
                MenuID = siparisDetay.MenuID,
                Menu = existedMenu,
                Siparis = existedSiparis
            };

            await _siparisDetayRepository.AddAsync(s); 
            return CreatedAtAction(nameof(GetSiparisDetayById), new { id = s.Id }, siparisDetay);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSiparisDetay(Guid id, [FromBody] UpdateSiparisDetayRequest siparisDetay)
        {
            var existedSiparisDetay = await _siparisDetayRepository.GetByIdAsync(id);

            if (existedSiparisDetay == null) 
            {
                return BadRequest("Sipariş Detay ID'si uyuşmazlığı");
            }

            if (siparisDetay.MenuID.HasValue)
            {
                var menu = await _menuRepository.GetByIdAsync(siparisDetay.MenuID.Value);
                if(menu == null)
                {
                    return BadRequest("Menu ID'si uyuşmazlığı");
                }
                existedSiparisDetay.Menu = menu;
            }

            if (siparisDetay.SiparisID.HasValue)
            {
                var siparis = await _siparisRepository.GetByIdAsync(siparisDetay.SiparisID.Value);
                if (siparis == null)
                {
                    return BadRequest("Sipariş ID'si uyuşmazlığı");
                }
                existedSiparisDetay.Siparis = siparis;
            }

            existedSiparisDetay.Adet = siparisDetay.Adet == null ? existedSiparisDetay.Adet: siparisDetay.Adet.Value;
            existedSiparisDetay.SiparisID = siparisDetay.SiparisID.HasValue ? siparisDetay.SiparisID.Value : existedSiparisDetay.SiparisID;
            existedSiparisDetay.MenuID = siparisDetay.MenuID.HasValue ? siparisDetay.MenuID.Value : existedSiparisDetay.MenuID;

            await _siparisDetayRepository.UpdateAsync(existedSiparisDetay); 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSiparisDetay(Guid id)
        {
            try
            {
                await _siparisDetayRepository.DeleteAsync(id); 
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }
    }
}
