using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgettoS6GestionaleHotelSabrinaCinque.DAO;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ProgettoS6GestionaleHotelSabrinaCinque.Controllers
{
    [Authorize(Policy = "GeneralAccessPolicy")]
    public class PrenotazioniController : Controller
    {
        private readonly IPrenotazioneDao _prenotazioneDao;
        private readonly IClienteDao _clienteDao;
        private readonly ICameraDao _cameraDao;
        private readonly IServizioDao _servizioDao;

        public PrenotazioniController(IPrenotazioneDao prenotazioneDao, IClienteDao clienteDao, ICameraDao cameraDao, IServizioDao servizioDao)
        {
            _prenotazioneDao = prenotazioneDao;
            _clienteDao = clienteDao;
            _cameraDao = cameraDao;
            _servizioDao = servizioDao;
        }

        public IActionResult Index()
        {
            var prenotazioni = _prenotazioneDao.GetAll();
            return View(prenotazioni);
        }

        public IActionResult Details(int id)
        {
            var prenotazione = _prenotazioneDao.GetById(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            prenotazione.Servizi = _servizioDao.GetByPrenotazioneId(id).ToList();
            return View(prenotazione);
        }

        public IActionResult Create()
        {
            ViewBag.Clienti = _clienteDao.GetAll();
            ViewBag.Camere = _cameraDao.GetAll();
            ViewBag.Servizi = _servizioDao.GetAll();

            var prenotazione = new Prenotazione
            {
                Anno = 2024,
                NumeroProgressivo = _prenotazioneDao.GetLastId() + 1 // Impostare il numero progressivo come ultimo ID + 1
            };

            return View(prenotazione);
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
            ViewBag.Servizi = _servizioDao.GetAll();
            return View(prenotazione);
        }

        public IActionResult Edit(int id)
        {
            var prenotazione = _prenotazioneDao.GetById(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            prenotazione.ServiziSelezionati = _servizioDao.GetByPrenotazioneId(id).Select(s => s.Id).ToList();

            ViewBag.Clienti = _clienteDao.GetAll();
            ViewBag.Camere = _cameraDao.GetAll();
            ViewBag.Servizi = _servizioDao.GetAll();
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
            ViewBag.Servizi = _servizioDao.GetAll();
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

        [HttpGet]
        public IActionResult GetCameraPrice(int id)
        {
            var camera = _cameraDao.GetById(id);
            if (camera == null)
            {
                return NotFound();
            }
            return Json(camera.Prezzo);
        }

        [HttpGet]
        public IActionResult Checkout(int id)
        {
            var prenotazione = _prenotazioneDao.GetById(id);
            if (prenotazione == null)
            {
                return NotFound();
            }

            prenotazione.Servizi = _servizioDao.GetByPrenotazioneId(id).ToList();

            var giorniSoggiorno = (prenotazione.Al - prenotazione.Dal).Days;
            var totaleStanza = prenotazione.Tariffa * giorniSoggiorno;
            var totaleServizi = prenotazione.Servizi.Sum(s => s.Prezzo);
            var totale = totaleStanza + totaleServizi - prenotazione.Caparra;

            var viewModel = new CheckoutViewModel
            {
                Prenotazione = prenotazione,
                TotaleStanza = totaleStanza,
                TotaleServizi = totaleServizi,
                Totale = totale
            };

            return View("Checkout", viewModel);
        }


        public IActionResult DownloadPdf(int id) //ho installato un pacchetto Install-Package iTextSharp.LGPLv2.Core per creare il pdf quando si va su checkout
        {
            var prenotazione = _prenotazioneDao.GetById(id);
            if (prenotazione == null)
            {
                return NotFound();
            }

            prenotazione.Servizi = _servizioDao.GetByPrenotazioneId(id).ToList();

            var checkoutViewModel = new CheckoutViewModel
            {
                Prenotazione = prenotazione,
                TotaleStanza = (prenotazione.Al - prenotazione.Dal).Days * prenotazione.Tariffa,
                TotaleServizi = prenotazione.Servizi.Sum(s => s.Prezzo),
                Totale = ((prenotazione.Al - prenotazione.Dal).Days * prenotazione.Tariffa) + prenotazione.Servizi.Sum(s => s.Prezzo) - prenotazione.Caparra
            };

            using (var ms = new MemoryStream())
            {
                var document = new Document();
                PdfWriter.GetInstance(document, ms);
                document.Open();

                document.Add(new Paragraph("Riepilogo Check-out"));
                document.Add(new Paragraph($"Cliente: {checkoutViewModel.Prenotazione.Cliente.Cognome} {checkoutViewModel.Prenotazione.Cliente.Nome}"));
                document.Add(new Paragraph($"Camera: {checkoutViewModel.Prenotazione.Camera.Descrizione}"));
                document.Add(new Paragraph($"Dal: {checkoutViewModel.Prenotazione.Dal.ToShortDateString()}"));
                document.Add(new Paragraph($"Al: {checkoutViewModel.Prenotazione.Al.ToShortDateString()}"));
                document.Add(new Paragraph($"Tariffa giornaliera: {checkoutViewModel.Prenotazione.Tariffa.ToString("C")}"));
                document.Add(new Paragraph($"Caparra: {checkoutViewModel.Prenotazione.Caparra.ToString("C")}"));

                document.Add(new Paragraph("Servizi Aggiuntivi:"));
                foreach (var servizio in checkoutViewModel.Prenotazione.Servizi)
                {
                    document.Add(new Paragraph($"{servizio.Descrizione} - {servizio.Prezzo.ToString("C")}"));
                }

                document.Add(new Paragraph($"Totale Stanza: {checkoutViewModel.TotaleStanza.ToString("C")}"));
                document.Add(new Paragraph($"Totale Servizi Aggiuntivi: {checkoutViewModel.TotaleServizi.ToString("C")}"));
                document.Add(new Paragraph($"Caparra Iniziale: {checkoutViewModel.Prenotazione.Caparra.ToString("C")}"));
                document.Add(new Paragraph($"Totale Da Saldare: {checkoutViewModel.Totale.ToString("C")}"));

                document.Close();

                return File(ms.ToArray(), "application/pdf", "Riepilogo_Checkout.pdf");
            }
        }


    }

}

