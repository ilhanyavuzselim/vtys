using Common.Requests.Masa;
using Common.Requests.Musteri;
using Common.Requests.Siparis;
using Domain.masa;
using Domain.musteri;
using Domain.personel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity;

namespace MVC.Controllers
{
    public class MasaController : Controller
    {
        private readonly ApiRequestHelper _apiRequestHelper;

        public MasaController(ApiRequestHelper apiRequestHelper)
        {
            _apiRequestHelper = apiRequestHelper;
        }

        // GET: MasaController
        public async Task<ActionResult> Index(int page = 1)
        {
            int pageSize = 16;
            int totalMasalar = 0;
            int totalPages = 0;
            var result = await _apiRequestHelper.GetAsync<IEnumerable<Masa>>(ApiEndpoints.MasaControllerGetUrl);
            if (result != null)
            {
                totalMasalar = result.Count();
                totalPages = (int)Math.Ceiling((double)totalMasalar / pageSize); 
                result = result.OrderBy(r => r.MasaNo);
                result = result.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(result);
        }

        // GET: MasaController/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var result = await _apiRequestHelper.GetAsync<Masa>(ApiEndpoints.MasaControllerGetByIdUrl + id);
            return View(result);
        }

        // GET: MasaController/Create
        public ActionResult _Create()
        {
            return View();
        }

        // POST: MasaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateMasaRequest masa)
        {

            await _apiRequestHelper.PostAsync<CreateMasaRequest>(masa, ApiEndpoints.MasaControllerCreateUrl);
            return RedirectToAction("Index");
        }

        // GET: MasaController/Edit/5
        public async Task<ActionResult> _EditAsync(Guid id)
        {
            var result = await _apiRequestHelper.GetAsync<Masa>(ApiEndpoints.MasaControllerGetByIdUrl + id);
            return View(result);
        }

        // POST: MasaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(UpdateMasaRequest masa)
        {
            await _apiRequestHelper.PutAsync(masa, ApiEndpoints.MasaControllerUpdateUrl + masa.Id);
            return RedirectToAction(nameof(Index));
        }

        // GET: MasaController/Delete/5
        public async Task<ActionResult> _DeleteAsync(Guid id)
        {
            await DeleteAsync(id);
            return RedirectToAction("Index");
        }

        // POST: MasaController/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task DeleteAsync(Guid id)
        {
            await _apiRequestHelper.DeleteAsync(ApiEndpoints.MasaControllerDeleteUrl + id);
        }

        [HttpGet]
        public async Task<IActionResult> _SiparisEkle(Guid id)
        {
            var masa = await _apiRequestHelper.GetAsync<Masa>(ApiEndpoints.MasaControllerGetByIdUrl + id);
            var personeller = await _apiRequestHelper.GetAsync<List<Personel>>(ApiEndpoints.PersonelControllerGetUrl);
            var musteriler = await _apiRequestHelper.GetAsync<List<Musteri>>(ApiEndpoints.MusteriControllerGetUrl);
            ViewBag.Personeller = personeller;
            ViewBag.Musteriler = musteriler;
            return PartialView("_siparisEkle", masa); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateSiparis(CreateSiparisRequest request)
        {
            if (ModelState.IsValid)
            {
                await _apiRequestHelper.PostAsync(request, ApiEndpoints.SiparisControllerCreateUrl);   
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult _MusteriEkle()
        {
            return PartialView("_musteriEkle");
        }

        [HttpPost]
        public async Task<IActionResult> CreateMusteri(CreateMusteriRequest musteri)
        {
            if (ModelState.IsValid)
            {
                await _apiRequestHelper.PostAsync(musteri, ApiEndpoints.MusteriControllerCreateUrl);
            }
            return RedirectToAction("Index");
        }
    }
}
