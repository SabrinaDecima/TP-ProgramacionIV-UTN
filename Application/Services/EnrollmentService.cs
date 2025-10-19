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
            var user = _userRepository.GetUserWithClasses(request.UserId);

            if (user == null)
            {
                return new EnrollmentResponse
                {
                    Success = false,
                    Message = "Usuario no encontrado."
                };
            }

            var gymClass = _gymClassRepository.GetById(request.GymClassId);

            if (gymClass == null)
            {
                return new EnrollmentResponse
                {
                    Success = false,
                    Message = "Clase no encontrada."
                };
            }

            if (!CanEnroll(user))
            {
                return new EnrollmentResponse
                {
                    Success = false,
                    Message = "No puede inscribirse a más clases según su plan actual."
                };
            }

            if(gymClass.Users.Count >= gymClass.MaxCapacity)
            {
                return new EnrollmentResponse
                {
                    Success = false,
                    Message = "La clase está completa."
                };
            }

            if (user.GymClasses == null)
            {
                user.GymClasses = new List<GymClass>();
            }

            if (user.GymClasses.Any(c => c.Id == gymClass.Id))
            {
                return new EnrollmentResponse
                {
                    Success = false,
                    Message = "Ya está inscrito en esta clase."
                };
            }

            if (gymClass.Users == null)
            {
                gymClass.Users = new List<User>();
            }

            
                user.GymClasses.Add(gymClass);

                gymClass.Users.Add(user);

            var updated = _userRepository.UpdateUser(user.Id, user);

            return new EnrollmentResponse
            {
                Success = updated,
                Message = updated ? "Inscripción exitosa." : "Error al actualizar la inscripción."
            };

        }

        public EnrollmentResponse UnenrollUser(EnrollUserRequest request)
        {
            var user = _userRepository.GetUserWithClasses(request.UserId);
            var gymClass = _gymClassRepository.GetById(request.GymClassId);

            if (user == null)
                return new EnrollmentResponse { Success = false, Message = "Usuario no encontrado." };

            if (gymClass == null)
                return new EnrollmentResponse { Success = false, Message = "Clase no encontrada." };

            if (user.GymClasses == null || gymClass.Users == null)
                return new EnrollmentResponse { Success = false, Message = "No hay inscripciones registradas." };

            var enrolledInClass = user.GymClasses.Any(c => c.Id == gymClass.Id);
            var userInClassUsers = gymClass.Users.Any(u => u.Id == user.Id);


            if (!enrolledInClass && !userInClassUsers)
                return new EnrollmentResponse { Success = false, Message = "El usuario no está inscrito en esta clase." };

            if (enrolledInClass)
                user.GymClasses.RemoveAll(c => c.Id == gymClass.Id);

            if (userInClassUsers)
                gymClass.Users.RemoveAll(u => u.Id == user.Id);


            var updated = _userRepository.UpdateUser(user.Id, user);

            return new EnrollmentResponse
            {
                Success = updated,
                Message = updated ? "Baja de clase exitosa." : "Error al procesar la baja."
            };
        }

        private bool CanEnroll(User user)
        {
            if (user.PlanId == null)
                return false;

            var limite = user.PlanId switch
            {
                1 => 1, //plan basico 1 clase por semana
                2 => 2, //plan premium 2
                3 => 3, //plan elite 3
            };

            return (user.GymClasses?.Count ?? 0) < limite;
        }


    }
}