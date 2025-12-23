using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseProjectYacenko.Models;
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
            try
            {
                var tariff = await _tariffService.GetTariffAsync(id);
                return View(tariff);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Subscribe(int tariffId)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var success = await _tariffService.AssignTariffToUserAsync(tariffId, userId);

            if (success)
                TempData["SuccessMessage"] = "Тариф успешно подключен!";
            else
                TempData["ErrorMessage"] = "Ошибка при подключении тарифа";

            return RedirectToAction("MyTariffs", "Profile");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Unsubscribe(int tariffId)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var success = await _tariffService.RemoveTariffFromUserAsync(tariffId, userId);

            if (success)
                TempData["SuccessMessage"] = "Тариф успешно отключен!";
            else
                TempData["ErrorMessage"] = "Ошибка при отключении тарифа";

            return RedirectToAction("MyTariffs", "Profile");
        }

        [HttpGet]
        public IActionResult Calculator()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Calculator(int minutes, int internet, int sms)
        {
            var matchingTariffs = await _tariffService.FindMatchingTariffsAsync(minutes, internet, sms);
            return View("Index", matchingTariffs);
        }
    }
}