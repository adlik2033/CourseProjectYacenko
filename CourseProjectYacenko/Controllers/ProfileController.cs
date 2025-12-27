using CourseProjectYacenko.DTO.User;
using CourseProjectYacenko.Models;
using CourseProjectYacenko.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseProjectYacenko.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITariffService _tariffService;
        private readonly IUserService _userService;

        public ProfileController(
            IAuthService authService,
            ITariffService tariffService,
            IUserService userService)
        {
            _authService = authService;
            _tariffService = tariffService;
            _userService = userService;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        // Главная страница профиля
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var user = await _authService.GetCurrentUserAsync(userId);

            if (user == null)
                return RedirectToAction("Login", "Account");

            return View(user);
        }

        // Страница редактирования профиля (GET)
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = GetCurrentUserId();
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var editModel = new EditProfileDto
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Address = user.Address,
                PassportData = user.PassportData
            };

            return View(editModel);
        }

        // Сохранение изменений профиля (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var userId = GetCurrentUserId();
                var success = await _userService.UpdateUserAsync(userId, model);

                if (success)
                {
                    TempData["SuccessMessage"] = "Профиль успешно обновлен!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ошибка при обновлении профиля");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
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