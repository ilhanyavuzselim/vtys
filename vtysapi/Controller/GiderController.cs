using Domain.gider;
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
    }
}
