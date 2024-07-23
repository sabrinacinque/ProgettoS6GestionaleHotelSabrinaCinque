using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgettoS6GestionaleHotelSabrinaCinque.DAO;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;
using System.Diagnostics;

namespace ProgettoS6GestionaleHotelSabrinaCinque.Controllers
{
    [Authorize(Policy = "GeneralAccessPolicy")]
    public class CamereController : Controller
    {
        private readonly ICameraDao _cameraDao;

        public CamereController(ICameraDao cameraDao)
        {
            _cameraDao = cameraDao;
        }

        public IActionResult Index()
        {
            try
            {
                var camere = _cameraDao.GetAll();
                return View(camere);
            }
            catch (Exception ex)
            {
                // Log dell'errore (opzionale)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult Details(int id)
        {
            var camera = _cameraDao.GetById(id);
            if (camera == null)
            {
                return NotFound();
            }
            return View(camera);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Camera camera)
        {
            if (ModelState.IsValid)
            {
                _cameraDao.Add(camera);
                return RedirectToAction(nameof(Index));
            }
            return View(camera);
        }

        public IActionResult Edit(int id)
        {
            var camera = _cameraDao.GetById(id);
            if (camera == null)
            {
                return NotFound();
            }
            return View(camera);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Camera camera)
        {
            if (id != camera.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _cameraDao.Update(camera);
                return RedirectToAction(nameof(Index));
            }
            return View(camera);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _cameraDao.Delete(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log dell'errore (opzionale)
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
