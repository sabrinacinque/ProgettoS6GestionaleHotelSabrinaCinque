using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgettoS6GestionaleHotelSabrinaCinque.DAO;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;
using System.Diagnostics;

namespace ProgettoS6GestionaleHotelSabrinaCinque.Controllers
{
    [Authorize(Policy = "GeneralAccessPolicy")]
    public class PrenotazioniController : Controller
    {
        private readonly IPrenotazioneDao _prenotazioneDao;
        private readonly IClienteDao _clienteDao;
        private readonly ICameraDao _cameraDao;

        public PrenotazioniController(IPrenotazioneDao prenotazioneDao, IClienteDao clienteDao, ICameraDao cameraDao)
        {
            _prenotazioneDao = prenotazioneDao;
            _clienteDao = clienteDao;
            _cameraDao = cameraDao;
        }

        public IActionResult Index()
        {
            try
            {
                var prenotazioni = _prenotazioneDao.GetAll();
                return View(prenotazioni);
            }
            catch (Exception ex)
            {
                // Log dell'errore (opzionale)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult Details(int id)
        {
            var prenotazione = _prenotazioneDao.GetById(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            return View(prenotazione);
        }

        public IActionResult Create()
        {
            ViewBag.Clienti = _clienteDao.GetAll();
            ViewBag.Camere = _cameraDao.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Prenotazione prenotazione)
        {
            if (ModelState.IsValid)
            {
                _prenotazioneDao.Add(prenotazione);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clienti = _clienteDao.GetAll();
            ViewBag.Camere = _cameraDao.GetAll();
            return View(prenotazione);
        }

        public IActionResult Edit(int id)
        {
            var prenotazione = _prenotazioneDao.GetById(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            ViewBag.Clienti = _clienteDao.GetAll();
            ViewBag.Camere = _cameraDao.GetAll();
            return View(prenotazione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Prenotazione prenotazione)
        {
            if (id != prenotazione.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _prenotazioneDao.Update(prenotazione);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clienti = _clienteDao.GetAll();
            ViewBag.Camere = _cameraDao.GetAll();
            return View(prenotazione);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _prenotazioneDao.Delete(id);
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
