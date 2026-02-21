using Application.Abstraction;
using Application.Interfaces;
using Application.Services;
using Application.Services.Implementations;
using Contracts.User.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationAPI_TP.Controllers
{
    [Route("api/admin/user")]
    [ApiController]
    [Authorize(Roles = "Administrador,SuperAdministrador")]
    public class AdminUserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGymClassService _gymClassService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPaymentService _paymentService;
        private readonly IHistoricalService _historicalService;

        public AdminUserController(IUserService userService, IGymClassService gymClassService, ISubscriptionService subscriptionService, IPaymentService paymentService, IHistoricalService historicalService)
        {
            _userService = userService;
            _gymClassService = gymClassService;
            _subscriptionService = subscriptionService;
            _paymentService = paymentService;
            _historicalService = historicalService;
        }


        [Authorize(Roles = "Administrador, SuperAdministrador")]

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();

            return Ok(users);
        }


        [Authorize(Roles = "Administrador, SuperAdministrador")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateUserByAdminRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = _userService.CreateUserByAdmin(request);

            if (!created)
                return BadRequest(new { message = "No se pudo crear el usuario. Verifica datos y plan/rol válidos." });

            return Ok(new { message = "Usuario creado correctamente." });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateByAdmin(int id, [FromBody] UpdateUserByAdminRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = _userService.UpdateUserByAdmin(id, request);
            if (!updated)
                return BadRequest(new { message = "No se pudo actualizar el usuario. Verifica datos y plan/rol válidos." });

            return Ok(new { message = "Usuario actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _userService.DeleteUser(id);
            if (!deleted)
                return NotFound("Usuario no encontrado.");

            return Ok(new { message = "Usuario eliminado correctamente." });
        }

        [Authorize(Roles = "SuperAdministrador")]
        [HttpGet("activity-summary")]
        public async Task<IActionResult> GetActivitySummary()
        {
            var allUsers = _userService.GetAll();
            var allClasses = _gymClassService.GetAll(0);
            var allPayments = await _paymentService.GetAllPaymentsAsync();

            var activeSubscriptionsCount = 0;
            var expiredSubscriptionsCount = 0;
            foreach (var user in allUsers.Where(u => u.RoleId == 1))
            {
                var sub = await _subscriptionService.GetActiveSubscriptionAsync(user.Id);
                if (sub != null)
                {
                    if (sub.IsActive && sub.EndDate > DateTime.Now)
                        activeSubscriptionsCount++;
                    else
                        expiredSubscriptionsCount++;
                }
            }

            var totalRevenue = allPayments
                .Where(p => p.Pagado)
                .Sum(p => p.Monto);

            var classesWithAvailableSpots = allClasses
                .Count(c => c.CurrentEnrollments < c.MaxCapacity);
            var fullClasses = allClasses
                .Count(c => c.CurrentEnrollments >= c.MaxCapacity);

            var stats = new
            {
                TotalUsers = allUsers.Count,
                TotalAdmins = allUsers.Count(u => u.RoleId == 2 || u.RoleId == 3),
                TotalSocios = allUsers.Count(u => u.RoleId == 1),

                ActiveSubscriptions = activeSubscriptionsCount,
                ExpiredSubscriptions = expiredSubscriptionsCount,
                SubscriptionRate = allUsers.Count(u => u.RoleId == 1) > 0
                    ? Math.Round((double)activeSubscriptionsCount / allUsers.Count(u => u.RoleId == 1) * 100, 2)
                    : 0,

                TotalClasses = allClasses.Count,
                ClassesWithAvailableSpots = classesWithAvailableSpots,
                FullClasses = fullClasses,
                OccupancyRate = allClasses.Any()
                    ? Math.Round((double)allClasses.Sum(c => c.CurrentEnrollments) /
                                 allClasses.Sum(c => c.MaxCapacity) * 100, 2)
                    : 0,

                TotalPayments = allPayments.Count,
                PaidPayments = allPayments.Count(p => p.Pagado),
                PendingPayments = allPayments.Count(p => !p.Pagado),
                TotalRevenue = totalRevenue,
            };

            return Ok(new { stats });
        }


    }
}
