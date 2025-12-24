using Microsoft.AspNetCore.Mvc;
using CourseProjectYacenko.Services;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Controllers
{
    public class TariffController : Controller
    {
        private readonly ITariffService _tariffService;

        public TariffController(ITariffService tariffService)
        {
            _tariffService = tariffService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tariffs = await _tariffService.GetAllTariffsAsync();
            return View(tariffs);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var tariff = await _tariffService.GetTariffAsync(id);
            if (tariff == null)
                return NotFound();

            return View(tariff);
        }
    }
}