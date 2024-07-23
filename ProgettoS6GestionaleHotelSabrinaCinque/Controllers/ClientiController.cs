using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProgettoS6GestionaleHotelSabrinaCinque.Controllers
{
    [Authorize(Policy = "GeneralAccessPolicy")]

    public class ClientiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
