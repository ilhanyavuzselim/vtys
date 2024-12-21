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
            var existedKategori = await _kategoriRepository.GetByIdAsync(menu.KategoriID);
            if(existedKategori == null)
            {
                return BadRequest("Verilen Kategori Bulunamadı");
            }
            
            Menu m = new Menu()
            {
                Ad = menu.Ad,
                Fiyat = menu.Fiyat,
                KategoriID = menu.KategoriID,
                Kategori = existedKategori
            };

            await _menuRepository.AddAsync(m); 
            return CreatedAtAction(nameof(GetMenuById), new { id = m.Id }, menu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(Guid id, [FromBody] UpdateMenuRequest menu)
        {
            var existedMenu = await _menuRepository.GetByIdAsync(id);

            if (existedMenu == null) 
            {
                return BadRequest("Menu ID'si uyuşmazlığı");
            }

            if (menu.KategoriID.HasValue)
            {
                var kategori = await _kategoriRepository.GetByIdAsync(menu.KategoriID.Value);
                if(kategori == null)
                {
                    return BadRequest("Kategori ID'si uyuşmazlığı");
                }
                existedMenu.Kategori = kategori;
            }

            existedMenu.Ad = menu.Ad == null ? existedMenu.Ad : menu.Ad;
            existedMenu.Fiyat = menu.Fiyat.HasValue ? menu.Fiyat.Value : existedMenu.Fiyat;

            await _menuRepository.UpdateAsync(existedMenu); 
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
