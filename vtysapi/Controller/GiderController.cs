using Common.Requests.Giderler;
using Domain.gider;
using Domain.rezervasyon;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.GiderController
{
    [Route("/api/[controller]")]
    [ApiController]
    public class GiderController : ControllerBase
    {
        private readonly IRepository<Gider> _giderlerRepository;
        public GiderController(IRepository<Gider> giderlerRepository)
        {
            _giderlerRepository = giderlerRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllGiderler()
        {
            var giderler = await _giderlerRepository.GetAllAsync();
            return Ok(giderler);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGider([FromBody] CreateGiderRequest gider)
        {
            if (gider == null)
            {
                return BadRequest("Gider verisi geçersiz");
            }

            Gider g = new Gider()
            {
                Ad = gider.Ad,
                Tarih = gider.Tarih,
                Tutar = gider.Tutar,
            };
            await _giderlerRepository.AddAsync(g);
            return CreatedAtAction(nameof(CreateGider), g);
        }
    }
}
