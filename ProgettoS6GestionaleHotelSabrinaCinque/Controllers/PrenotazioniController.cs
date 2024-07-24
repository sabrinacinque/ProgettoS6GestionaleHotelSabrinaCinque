using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProgettoS6GestionaleHotelSabrinaCinque.DAO;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProgettoS6GestionaleHotelSabrinaCinque.Controllers
{
    public class PrenotazioniController : Controller
    {
        private readonly IPrenotazioneDao _prenotazioneDao;
        private readonly IClienteDao _clienteDao;
        private readonly ICameraDao _cameraDao;
        private readonly ILogger<PrenotazioniController> _logger;



        public PrenotazioniController(IPrenotazioneDao prenotazioneDao, IClienteDao clienteDao, ICameraDao cameraDao, ILogger<PrenotazioniController> logger)
        {
            _prenotazioneDao = prenotazioneDao;
            _clienteDao = clienteDao;
            _cameraDao = cameraDao;
            _logger = logger;

        }

        public IActionResult Index()
        {
            var prenotazioni = _prenotazioneDao.GetAll();
            return View(prenotazioni);
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
                prenotazione.Cliente = _clienteDao.GetById(prenotazione.ClienteId);
                prenotazione.Camera = _cameraDao.GetById(prenotazione.CameraId);
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
            _logger.LogInformation("Inizio metodo Edit");

            if (id != prenotazione.Id)
            {
                _logger.LogWarning("ID mismatch: id != prenotazione.Id");
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("ModelState is valid");
                    _logger.LogInformation($"Prenotazione ID: {prenotazione.Id}, ClienteId: {prenotazione.ClienteId}, CameraId: {prenotazione.CameraId}");

                    prenotazione.Cliente = _clienteDao.GetById(prenotazione.ClienteId);
                    prenotazione.Camera = _cameraDao.GetById(prenotazione.CameraId);

                    if (prenotazione.Cliente == null)
                    {
                        _logger.LogWarning("Cliente non trovato");
                        ModelState.AddModelError("ClienteId", "Cliente non trovato");
                    }

                    if (prenotazione.Camera == null)
                    {
                        _logger.LogWarning("Camera non trovata");
                        ModelState.AddModelError("CameraId", "Camera non trovata");
                    }

                    if (prenotazione.Cliente != null && prenotazione.Camera != null)
                    {
                        _prenotazioneDao.Update(prenotazione);
                        _logger.LogInformation("Prenotazione aggiornata con successo");
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Errore durante l'aggiornamento della prenotazione: {ex.Message}");
                    ModelState.AddModelError("", "Errore durante l'aggiornamento della prenotazione.");
                }
            }
            else
            {
                _logger.LogWarning("ModelState non è valido.");
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        _logger.LogWarning($"Errore nel campo {modelState.Key}: {error.ErrorMessage}");
                    }
                }
            }

            ViewBag.Clienti = _clienteDao.GetAll();
            ViewBag.Camere = _cameraDao.GetAll();
            return View(prenotazione);
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
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
