using Common.Requests.Masa;
using Domain.masa;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> Index()
        {
            var result = await _apiRequestHelper.GetAsync<IEnumerable<Masa>>(ApiEndpoints.MasaControllerGetUrl);
            if (result != null)
            {
                result = result.OrderBy(r => r.MasaNo);
            }
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
    }
}
