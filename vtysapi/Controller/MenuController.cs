using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.menu;
using Common.Requests.Menu;
using Domain.kategori;

namespace WebApi.Controllers.MenuController
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<Kategori> _kategoriRepository;

        public MenuController(IRepository<Menu> menuRepository, IRepository<Kategori> kategoriRepository)
        {
            _menuRepository = menuRepository;
            _kategoriRepository = kategoriRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenuler()
        {
            var menuler = await _menuRepository.GetAllAsync(m => m.Kategori);
            return Ok(menuler);  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuById(Guid id)
        {
            try
            {
                var menu = await _menuRepository.GetByIdAsync(id, m=>m.Kategori);
                return Ok(menu);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] CreateMenuRequest menu)
        {
            if (menu == null)
            {
                return BadRequest("Masa verisi geçersiz");
            }
            var k = await _kategoriRepository.GetByIdAsync(menu.KategoriID);
            if(k == null)
            {
                return BadRequest("Verilen Kategori Bulunamadı");
            }
            
            Menu m = new Menu()
            {
                Ad = menu.Ad,
                Fiyat = menu.Fiyat,
                KategoriID = menu.KategoriID,
                Kategori = k
            };

            await _menuRepository.AddAsync(m); 
            return CreatedAtAction(nameof(GetMenuById), new { id = m.Id }, menu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(Guid id, [FromBody] UpdateMenuRequest menu)
        {
            var existedMasa = await _menuRepository.GetByIdAsync(id);

            if (existedMasa == null) 
            {
                return BadRequest("Menu ID'si uyuşmazlığı");
            }

            if (menu.KategoriID.HasValue)
            {
                var k = await _kategoriRepository.GetByIdAsync(menu.KategoriID.Value);
                if(k == null)
                {
                    return BadRequest("Kategori ID'si uyuşmazlığı");
                }
                existedMasa.Kategori = k;
            }

            existedMasa.Ad = menu.Ad == null ? existedMasa.Ad : menu.Ad;
            existedMasa.Fiyat = menu.Fiyat.HasValue ? menu.Fiyat.Value : existedMasa.Fiyat;

            await _menuRepository.UpdateAsync(existedMasa); 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(Guid id)
        {
            try
            {
                await _menuRepository.DeleteAsync(id); 
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
        }
    }
}
