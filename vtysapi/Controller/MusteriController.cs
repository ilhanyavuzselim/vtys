using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.kisi;
using Domain.musteri;
using Common.Requests.Musteri;

namespace WebApi.Controllers.MusteriController
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusteriController : ControllerBase
    {
        private readonly IRepository<Musteri> _musteriRepository;
        private readonly IRepository<Kisi> _kisiRepository;

        public MusteriController(IRepository<Musteri> musteriRepository, IRepository<Kisi> kisiRepository)
        {
            _musteriRepository = musteriRepository;
            _kisiRepository = kisiRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMusteriler()
        {
            var musteriler = await _musteriRepository.GetAllAsync();
            return Ok(musteriler.GroupBy(p => p.Id).Select(p => p.First()).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMusteriById(Guid id)
        {
            try
            {
                var musteri = await _musteriRepository.GetByIdAsync(id);
                return Ok(musteri);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMusteri([FromBody] CreateMusteriRequest musteri)
        {
            if (musteri == null)
            {
                return BadRequest("Müşteri verisi geçersiz");
            }

            if (musteri.KisiId.HasValue)
            {
                var existedKisi = await _kisiRepository.GetByIdAsync(musteri.KisiId.Value);
                if (existedKisi == null)
                {
                    return NotFound("Verilen Kişi ID'ye sahip kişi bulunamadı");
                }
                Dictionary<string, object> d = new Dictionary<string, object>
                {
                    { "p_kisi_id", musteri.KisiId },
                    { "p_telefon", musteri.Telefon}
                };
                await _musteriRepository.ExecuteStoredProcedureAsync("create_musteri_via_existed_kisi", d);
            }
            else
            {
                if(musteri.Ad == null || musteri.Soyad == null)
                {
                    return BadRequest("Müşteri verisi geçersiz");
                }
                Dictionary<string, object> d = new Dictionary<string, object>()
                {
                    {"p_ad" , musteri.Ad },
                    {"p_soyad" , musteri.Soyad },
                    {"p_telefon", musteri.Telefon }
                };
                await _musteriRepository.ExecuteStoredProcedureAsync("insert_musteri", d);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMusteri(Guid id, [FromBody] UpdateMusteriRequest musteri)
        {
            if(musteri == null)
            {
                return BadRequest("Verilen Parametreler Geçersiz");
            }
            var existedMusteri = await _musteriRepository.GetByIdAsync(id);
            if (existedMusteri == null)
            {
                return NotFound("Verilen Kişi ID'ye sahip kişi bulunamadı");
            }
            if(musteri.Ad != null) existedMusteri.Ad = musteri.Ad;
            if(musteri.Soyad != null) existedMusteri.Soyad = musteri.Soyad;
            if(musteri.Telefon != null) existedMusteri.Telefon = musteri.Telefon;
            Dictionary<string, object> d = new Dictionary<string, object>()
            {
                {"p_id", id },
                {"p_ad", existedMusteri.Ad },
                {"p_soyad", existedMusteri.Soyad },
                {"p_telefon", existedMusteri.Telefon }
            };
            await _musteriRepository.ExecuteStoredProcedureAsync("update_musteri", d);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusteri(Guid id)
        {
            try
            {
                Dictionary<string, object> d = new Dictionary<string, object>()
                {
                    {"p_id", id },
                };
                await _musteriRepository.ExecuteStoredProcedureAsync("remove_musteri", d);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
