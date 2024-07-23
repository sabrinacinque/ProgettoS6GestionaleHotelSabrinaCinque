using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgettoS6GestionaleHotelSabrinaCinque.DAO;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;
using System.Diagnostics;

namespace ProgettoS6GestionaleHotelSabrinaCinque.Controllers
{
    [Authorize(Policy = "GeneralAccessPolicy")]
    public class ClientiController : Controller
    {
        private readonly IClienteDao _clienteDao;

        public ClientiController(IClienteDao clienteDao)
        {
            _clienteDao = clienteDao;
        }

        public IActionResult Index()
        {
            try
            {
                var clienti = _clienteDao.GetAll();
                return View(clienti);
            }
            catch (Exception ex)
            {
                // Log dell'errore (opzionale)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult Details(int id)
        {
            var cliente = _clienteDao.GetById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _clienteDao.Add(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public IActionResult Edit(int id)
        {
            var cliente = _clienteDao.GetById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _clienteDao.Update(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _clienteDao.Delete(id);
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
