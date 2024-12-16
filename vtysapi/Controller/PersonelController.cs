﻿using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.kisi;
using Domain.personel;
using Common.Requests.Personel;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonelController : ControllerBase
    {
        private readonly IRepository<Personel> _personelRepository;
        private readonly IRepository<Kisi> _kisiRepository;

        public PersonelController(IRepository<Personel> personelRepository, IRepository<Kisi> kisiRepository)
        {
            _personelRepository = personelRepository;
            _kisiRepository = kisiRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersoneller()
        {
            var personeller = await _personelRepository.GetAllAsync();
            return Ok(personeller.GroupBy(p=>p.Id).Select(p => p.First()).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonelById(Guid id)
        {
            try
            {
                var personel = await _personelRepository.GetByIdAsync(id);
                return Ok(personel);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonel([FromBody] CreatePersonelRequest personel)
        {
            if (personel == null)
            {
                return BadRequest("Personel verisi geçersiz");
            }

            if(personel.KisiId != Guid.Empty)
            {
                var existedKisi = await _kisiRepository.GetByIdAsync(personel.KisiId);
                if(existedKisi == null)
                {
                    return NotFound("Verilen Kişi ID'ye sahip kişi bulunamadı");
                }
                Dictionary<string,object> d = new Dictionary<string, object>
                {
                    { "p_kisi_id", personel.KisiId },
                    { "p_pozisyon", personel.Pozisyon }
                };
                await _personelRepository.ExecuteStoredProcedureAsync("create_personel_via_existed_kisi", d);
            }
            else
            {
                Dictionary<string, object> d = new Dictionary<string, object>()
                {
                    {"p_ad" , personel.Ad },
                    {"p_soyad" , personel.Soyad },
                    {"p_pozisyon", personel.Pozisyon }
                };
                await _personelRepository.ExecuteStoredProcedureAsync("insert_personel", d);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonel(Guid id, [FromBody] UpdatePersonelRequest personel)
        {
            if (id != personel.id || id != personel.KisiId || personel.KisiId != personel.id)
            {
                return BadRequest("Personel ID'si ile URL uyuşmazlığı");
            }
            var existedKisi = await _personelRepository.GetByIdAsync(personel.id);
            if (existedKisi == null)
            {
                return NotFound("Verilen Kişi ID'ye sahip kişi bulunamadı");
            }
            Dictionary<string, object> d = new Dictionary<string, object>()
            {
                {"p_id", personel.id },
                {"p_ad", personel.Ad },
                {"p_soyad", personel.Soyad },
                {"p_pozisyon", personel.Pozisyon }
            };
            await _personelRepository.ExecuteStoredProcedureAsync("update_personel", d);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonel(Guid id)
        {
            try
            {
                await _personelRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
