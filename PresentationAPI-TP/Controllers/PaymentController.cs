using Application.Abstraction;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public ActionResult<List<Payment>> GetAll() => Ok(_paymentService.GetAllPayments());

        [HttpGet("{id}")]
        public ActionResult<Payment> GetById(int id)
        {
            var payment = _paymentService.GetPaymentById(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpPost]
        public ActionResult<Payment> Create(Payment payment)
        {
            var createdPayment = _paymentService.CreatePayment(payment);
            return CreatedAtAction(nameof(GetById), new { id = createdPayment.Id }, createdPayment);
        }

        [HttpPut("{id}")]
        public ActionResult<Payment> Update(int id, Payment payment)
        {
            if (id != payment.Id) return BadRequest();
            var updatedPayment = _paymentService.UpdatePayment(payment);
            return Ok(updatedPayment);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var result = _paymentService.DeletePayment(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public ActionResult<List<Payment>> GetPaymentsByUser(int userId)
            => Ok(_paymentService.GetPaymentsByUserId(userId));

        [HttpGet("user/{userId}/pending")]
        public ActionResult<List<Payment>> GetPendingPaymentsByUser(int userId)
            => Ok(_paymentService.GetPendingPaymentsByUserId(userId));
    }
}
