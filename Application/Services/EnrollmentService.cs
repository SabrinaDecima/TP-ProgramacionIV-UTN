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

        public bool EnrollUser(EnrollUserRequest request)
        {
            //try
            //{
            //    var user = _userRepository.GetUserWithClassesAndPayments(request.UserId);
            //    var gymClass = _gymClassRepository.GetById(request.GymClassId);

            //    if (user == null)
            //        return new EnrollmentResponse { Message = "Usuario no encontrado." };

            //    if (gymClass == null)
            //        return new EnrollmentResponse { Message = "Clase no encontrada." };

<<<<<<< HEAD
            //    if (!IsFutureOrToday(gymClass.Fecha))
            //        return new EnrollmentResponse { Message = "No se puede inscribir en clases pasadas." };

            //    if (!HasActivePlan(user))
            //        return new EnrollmentResponse { Message = "El usuario no tiene un plan activo." };
=======
                if (!HasActivePlan(user))
                    return new EnrollmentResponse { Message = "El usuario no tiene un plan activo." };
>>>>>>> ce6a040097a20c20044d1dc025e9a12cbb308c28

            //    if (user.GymClasses.Any(gc => gc.Id == gymClass.Id))
            //        return new EnrollmentResponse { Message = "El usuario ya está inscrito en esta clase." };

            //    if (!CanEnrollMoreClasses(user, gymClass.Fecha))
            //        return new EnrollmentResponse { Message = "Límite de inscripciones alcanzado para tu plan." };

            //    user.GymClasses.Add(gymClass);
            //    _userRepository.UpdateUser(user.Id, user);

            //    return new EnrollmentResponse { Message = "Inscripción exitosa." };
            //}
            //catch (Exception)
            //{
            //    return new EnrollmentResponse { Message = "Error al procesar la inscripción." };
            //}

            return _userRepository.EnrollUserToClass(request.UserId, request.GymClassId);
            
        }

        public bool UnenrollUser(EnrollUserRequest request)
        {

            return _userRepository.UnEnrollUserToClass(request.UserId, request.GymClassId);
        }

        // validaciones

<<<<<<< HEAD
        //private bool IsFutureOrToday(string dateStr)
        //{

        //    if (!DateTime.TryParseExact(dateStr, "yyyy-MM-dd", null, DateTimeStyles.None, out var classDate))
        //        return false;

        //    return classDate.Date >= DateTime.Today;
        //}
=======
>>>>>>> ce6a040097a20c20044d1dc025e9a12cbb308c28

        //private bool HasActivePlan(User user)
        //{
        //    if (user.RoleId != (int)TypeRole.Socio)
        //        return true;
        //    var lastPaidPayment = user.Pagos?
        //        .Where(p => p.Pagado)
        //        .OrderByDescending(p => p.Fecha).FirstOrDefault();

        //    if (lastPaidPayment == null) return false;

        //    if (!DateTime.TryParseExact(lastPaidPayment.Fecha, "yyyy-MM-dd", null, DateTimeStyles.None, out var expiryDate))
        //        return false;

        //    return expiryDate.Date >= DateTime.Today;
        //}

<<<<<<< HEAD
      
=======
        private bool CanEnrollMoreClasses(User user, string classDateStr)
        {
            if (!DateTime.TryParseExact(classDateStr, "yyyy-MM-dd", null, DateTimeStyles.None, out var classDate))
                return true;

            // Obtener la semana de la clase que se quiere inscribir
            int targetWeek = GetWeekOfYear(classDate);
            int targetYear = classDate.Year;
>>>>>>> ce6a040097a20c20044d1dc025e9a12cbb308c28

    //        // Calcular semana de la clase (lunes a domingo)
    //        var diff = (7 + (classDate.DayOfWeek - DayOfWeek.Monday)) % 7;
    //        var startOfWeek = classDate.AddDays(-diff).Date;
    //        var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);

<<<<<<< HEAD
    //        var enrolledThisWeek = user.GymClasses
    //            .Where(gc =>
    //            {
    //                if (!DateTime.TryParseExact(gc.Fecha, "yyyy-MM-dd", null, DateTimeStyles.None, out var gcDate))
    //                    return false;
    //                return gcDate >= startOfWeek && gcDate <= endOfWeek;
    //            })
    //            .Count();

    //        return enrolledThisWeek < maxClasses;
    //    }
    //
=======
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
>>>>>>> ce6a040097a20c20044d1dc025e9a12cbb308c28
    }
}