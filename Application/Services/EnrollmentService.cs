using Application.Abstraction;
using Application.Interfaces;
using Contracts.Enrollment.Request;
using Contracts.Enrollment.Response;
using Domain.Entities;
using System.Globalization;

namespace Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGymClassRepository _gymClassRepository;

        public EnrollmentService(
            IUserRepository userRepository,
            IGymClassRepository gymClassRepository)
        {
            _userRepository = userRepository;
            _gymClassRepository = gymClassRepository;
        }

        public EnrollmentResponse EnrollUser(EnrollUserRequest request)
        {
            try
            {
                var user = _userRepository.GetUserWithClassesAndPayments(request.UserId);
                var gymClass = _gymClassRepository.GetById(request.GymClassId);

                if (user == null)
                    return new EnrollmentResponse { Message = "Usuario no encontrado." };

                if (gymClass == null)
                    return new EnrollmentResponse { Message = "Clase no encontrada." };

                if (!HasActivePlan(user))
                    return new EnrollmentResponse { Message = "El usuario no tiene un plan activo." };

                if (user.GymClasses.Any(gc => gc.Id == gymClass.Id))
                    return new EnrollmentResponse { Message = "El usuario ya está inscrito en esta clase." };

                if (!CanEnrollMoreClasses(user, gymClass.Fecha))
                    return new EnrollmentResponse { Message = "Límite de inscripciones alcanzado para tu plan." };

                user.GymClasses.Add(gymClass);
                _userRepository.UpdateUser(user.Id, user);

                return new EnrollmentResponse { Message = "Inscripción exitosa." };
            }
            catch (Exception)
            {
                return new EnrollmentResponse { Message = "Error al procesar la inscripción." };
            }
        }

        public EnrollmentResponse UnenrollUser(EnrollUserRequest request)
        {
            try
            {
                var user = _userRepository.GetUserWithClassesAndPayments(request.UserId);
                if (user == null)
                    return new EnrollmentResponse { Message = "Usuario no encontrado." };

                var gymClass = user.GymClasses.FirstOrDefault(gc => gc.Id == request.GymClassId);
                if (gymClass == null)
                    return new EnrollmentResponse { Message = "El usuario no está inscrito en esta clase." };

                user.GymClasses.Remove(gymClass);
                _userRepository.UpdateUser(user.Id, user);

                return new EnrollmentResponse { Message = "Inscripción cancelada." };
            }
            catch (Exception)
            {
                return new EnrollmentResponse { Message = "Error al cancelar la inscripción." };
            }
        }

        // validaciones


        private bool HasActivePlan(User user)
        {
            if (user.RoleId != (int)TypeRole.Socio)
                return true;
            var lastPaidPayment = user.Pagos?
                .Where(p => p.Pagado)
                .OrderByDescending(p => p.Fecha).FirstOrDefault();

            if (lastPaidPayment == null) return false;

            if (!DateTime.TryParseExact(lastPaidPayment.Fecha, "yyyy-MM-dd", null, DateTimeStyles.None, out var expiryDate))
                return false;

            return expiryDate.Date >= DateTime.Today;
        }

        private bool CanEnrollMoreClasses(User user, string classDateStr)
        {
            if (!DateTime.TryParseExact(classDateStr, "yyyy-MM-dd", null, DateTimeStyles.None, out var classDate))
                return true;

            // Obtener la semana de la clase que se quiere inscribir
            int targetWeek = GetWeekOfYear(classDate);
            int targetYear = classDate.Year;

            int maxClasses = user.PlanId switch
            {
                1 => 2,  // Basic
                2 => 5,  // Premium
                3 => 10, // Elite
                _ => 2
            };

            // Contar cuántas clases YA TIENE el usuario en ESA SEMANA
            var enrolledThisWeek = user.GymClasses
                .Where(gc =>
                {
                    if (!DateTime.TryParseExact(gc.Fecha, "yyyy-MM-dd", null, DateTimeStyles.None, out var gcDate))
                        return false;
                    return GetWeekOfYear(gcDate) == targetWeek && gcDate.Year == targetYear;
                })
                .Count();

            return enrolledThisWeek < maxClasses;
        }

        private int GetWeekOfYear(DateTime date)
        {
            var calendar = System.Globalization.CultureInfo.InvariantCulture.Calendar;
            return calendar.GetWeekOfYear(
                date,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday // Lunes como primer día de la semana
            );
        }
    }
}