using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseProjectYacenko.Services;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITariffService _tariffService;

        public ProfileController(IUserService userService, ITariffService tariffService)
        {
            _userService = userService;
            _tariffService = tariffService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserProfileAsync(userId);

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> MyTariffs()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var tariffs = await _userService.GetUserTariffsAsync(userId);

            return View(tariffs);
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(int tariffId)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var success = await _tariffService.AssignTariffToUserAsync(tariffId, userId);

            if (success)
                TempData["SuccessMessage"] = "Тариф успешно подключен!";
            else
                TempData["ErrorMessage"] = "Ошибка при подключении тарифа";

            return RedirectToAction("MyTariffs");
        }

        [HttpPost]
        public async Task<IActionResult> Unsubscribe(int tariffId)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var success = await _tariffService.RemoveTariffFromUserAsync(tariffId, userId);

            if (success)
                TempData["SuccessMessage"] = "Тариф успешно отключен!";
            else
                TempData["ErrorMessage"] = "Ошибка при отключении тарифа";

            return RedirectToAction("MyTariffs");
        }
    }
}