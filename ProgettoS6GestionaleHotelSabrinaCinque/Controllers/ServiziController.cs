using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgettoS6GestionaleHotelSabrinaCinque.DAO;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;
using System.Diagnostics;

namespace ProgettoS6GestionaleHotelSabrinaCinque.Controllers
{
    [Authorize(Policy = "AdminPolicy")]//solo l'admin ha accesso al database dei servizi e fare le crud
    public class ServiziController : Controller
    {
        private readonly IServizioDao _servizioDao;

        public ServiziController(IServizioDao servizioDao)
        {
            _servizioDao = servizioDao;
        }

        public IActionResult Index()
        {
            try
            {
                var servizi = _servizioDao.GetAll();
                return View(servizi);
            }
            catch (Exception ex)
            {
                // Log dell'errore (opzionale)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult Details(int id)
        {
            var servizio = _servizioDao.GetById(id);
            if (servizio == null)
            {
                return NotFound();
            }
            return View(servizio);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Servizio servizio)
        {
            if (ModelState.IsValid)
            {
                _servizioDao.Add(servizio);
                return RedirectToAction(nameof(Index));
            }
            return View(servizio);
        }

        public IActionResult Edit(int id)
        {
            var servizio = _servizioDao.GetById(id);
            if (servizio == null)
            {
                return NotFound();
            }
            return View(servizio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Servizio servizio)
        {
            if (id != servizio.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _servizioDao.Update(servizio);
                return RedirectToAction(nameof(Index));
            }
            return View(servizio);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _servizioDao.Delete(id);
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
