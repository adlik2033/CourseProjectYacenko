using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseProjectYacenko.Services;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;

        public PaymentController(IPaymentService paymentService, IUserService userService)
        {
            _paymentService = paymentService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult AddBalance()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBalance(decimal amount, string paymentMethod = "Online")
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("", "Сумма должна быть больше нуля");
                return View();
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var payment = await _paymentService.CreatePaymentAsync(userId, amount, paymentMethod);

            if (payment != null)
                TempData["SuccessMessage"] = $"Баланс успешно пополнен на {amount} руб.!";
            else
                TempData["ErrorMessage"] = "Ошибка при пополнении баланса";

            return RedirectToAction("Index", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> MyPayments()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var payments = await _paymentService.GetPaymentsByUserAsync(userId);

            return View(payments);
        }
    }
}