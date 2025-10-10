
using Application.Abstraction;
using Contracts.GymClass.Request;
using Contracts.Payment.Request;
using Contracts.Payment.Response;
using Domain.Entities;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public List<PaymentResponse> GetAllPayments()
        {
            var payments = _paymentRepository.GetAll();
            return payments.Select(p => new PaymentResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                Monto = p.Monto,
                Fecha = p.Fecha,
                Pagado = p.Pagado
            }).ToList();
        }

        public PaymentResponse? GetPaymentById(int id)
        {
            var payment = _paymentRepository.GetById(id);
            if (payment == null) return null;

            return new PaymentResponse
            {
                Id = payment.Id,
                UserId = payment.UserId,
                Monto = payment.Monto,
                Fecha = payment.Fecha,
                Pagado = payment.Pagado
            };
        }

        public PaymentResponse? CreatePayment(CreatePaymentRequest request)
        {
            if (request.Monto <= 0)
                return null;

            var payment = new Payment
            {
                UserId = request.UserId,
                Monto = request.Monto,
                Fecha = request.Fecha ?? string.Empty,
                Pagado = request.Pagado
            };

            var created = _paymentRepository.CreatePayment(payment);
            if (created == null) return null;

            return new PaymentResponse
            {
                Id = created.Id,
                UserId = created.UserId,
                Monto = created.Monto,
                Fecha = created.Fecha,
                Pagado = created.Pagado
            };
        }

        public PaymentResponse? UpdatePayment(int id, UpdatePaymentRequest request)
        {
            var existing = _paymentRepository.GetById(id);
            if (existing == null)
                return null;

            existing.UserId = request.UserId;
            existing.Monto = request.Monto;
            existing.Fecha = request.Fecha ?? string.Empty;
            existing.Pagado = request.Pagado;

            var updated = _paymentRepository.UpdatePayment(existing);
            if (updated == null) return null;

            return new PaymentResponse
            {
                Id = updated.Id,
                UserId = updated.UserId,
                Monto = updated.Monto,
                Fecha = updated.Fecha,
                Pagado = updated.Pagado
            };
        }

        public bool DeletePayment(int id)
        {
            var existing = _paymentRepository.GetById(id);
            return existing != null && _paymentRepository.DeletePayment(id);
        }

        public List<PaymentResponse> GetPaymentsByUserId(int userId)
        {
            var payments = _paymentRepository.GetPaymentsByUserId(userId);
            return payments.Select(p => new PaymentResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                Monto = p.Monto,
                Fecha = p.Fecha,
                Pagado = p.Pagado
            }).ToList();
        }

        public List<PaymentResponse> GetPendingPaymentsByUserId(int userId)
        {
            var payments = _paymentRepository.GetPaymentsByUserId(userId);
            var pending = payments.Where(p => !p.Pagado).ToList();
            return pending.Select(p => new PaymentResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                Monto = p.Monto,
                Fecha = p.Fecha,
                Pagado = p.Pagado
            }).ToList();
        }
    }
}