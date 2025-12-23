using CourseProjectYacenko.Models;
using CourseProjectYacenko.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        private readonly ITariffService _tariffService;

        public ProfileController(
            IUserService userService,
            IPaymentService paymentService,
            ITariffService tariffService)
        {
            _userService = userService;
            _paymentService = paymentService;
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

        [HttpGet]
        public async Task<IActionResult> MyPayments()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var payments = await _userService.GetUserPaymentsAsync(userId);

            return View(payments);
        }

        [HttpGet]
        public IActionResult AddBalance()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBalance(decimal amount)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("", "Сумма должна быть больше нуля");
                return View();
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var success = await _userService.AddBalanceAsync(userId, amount);

            if (success)
                TempData["SuccessMessage"] = "Баланс успешно пополнен!";
            else
                TempData["ErrorMessage"] = "Ошибка при пополнении баланса";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UpdateUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var success = await _userService.UpdateUserProfileAsync(userId, model);

            if (success)
                TempData["SuccessMessage"] = "Профиль успешно обновлен!";
            else
                TempData["ErrorMessage"] = "Ошибка при обновлении профиля";

            return RedirectToAction("Index");
        }
    }
}