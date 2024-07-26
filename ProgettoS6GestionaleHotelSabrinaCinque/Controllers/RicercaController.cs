using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgettoS6GestionaleHotelSabrinaCinque.DAO;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;

namespace ProgettoS6GestionaleHotelSabrinaCinque.Controllers
{
    [Authorize(Policy = "GeneralAccessPolicy")]

    public class RicercaController : Controller
    {
        private readonly IPrenotazioneDao _prenotazioneDao;

        public RicercaController(IPrenotazioneDao prenotazioneDao)
        {
            _prenotazioneDao = prenotazioneDao;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RicercaPrenotazioniCliente(string codiceFiscale)
        {
            var prenotazioni = _prenotazioneDao.GetPrenotazioniByCodiceFiscale(codiceFiscale);
            return PartialView("_PrenotazioniListPartial", prenotazioni);
        }

        [HttpPost]
        public IActionResult RicercaNumeroPrenotazioniPensioneCompleta()
        {
            var totalePrenotazioni = _prenotazioneDao.GetTotalePrenotazioniPerTipologia("pensione completa");
            return Content($"Totale prenotazioni per soggiorni di tipo \"pensione completa\": {totalePrenotazioni}");
        }
    }
}
