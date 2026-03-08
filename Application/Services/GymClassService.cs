using Application.Abstraction;
using Application.Interfaces;
using Contracts.GymClass.Request;
using Contracts.GymClass.Response;
using Domain.Entities;

namespace Application.Services
{
    public class GymClassService : IGymClassService
    {
        private readonly IGymClassRepository _gymClassRepository;

        public GymClassService(IGymClassRepository gymClassRepository)
        {
            _gymClassRepository = gymClassRepository;
        }

        // 🔹 GET ALL con estado de reserva + cupos
        public List<GymClassResponse> GetAll(int userId)
        {
            var classes = _gymClassRepository.GetAllWithUsers();

            return classes.Select(gc => new GymClassResponse
            {
                Id = gc.Id,
                Nombre = gc.Nombre,
                Descripcion = gc.Descripcion,
                DuracionMinutos = gc.DuracionMinutos,
                ImageUrl = gc.ImageUrl,
                Dia = gc.Dia,
                Hora = gc.Hora,
                IsReservedByUser = gc.Users.Any(u => u.Id == userId),
                MaxCapacity = gc.MaxCapacityUser,           
                CurrentEnrollments = gc.Users.Count         
            }).ToList();
        }

        // 🔹 GET BY ID con estado de reserva + cupos
        public GymClassResponse? GetById(int id, int userId)
        {
            var gc = _gymClassRepository.GetByIdWithUsers(id);
            if (gc == null) return null;

            return new GymClassResponse
            {
                Id = gc.Id,
                Nombre = gc.Nombre,
                Descripcion = gc.Descripcion,
                DuracionMinutos = gc.DuracionMinutos,
                ImageUrl = gc.ImageUrl,
                Dia = gc.Dia,
                Hora = gc.Hora,
                IsReservedByUser = gc.Users.Any(u => u.Id == userId),
                MaxCapacity = gc.MaxCapacityUser,           
                CurrentEnrollments = gc.Users.Count         
            };
        }

        // 🔹 RESERVAR
        public bool ReserveClass(int classId, int userId)
        {
            var gymClass = _gymClassRepository.GetByIdWithUsers(classId);
            if (gymClass == null) return false;

            if (gymClass.Users.Any(u => u.Id == userId))
                return true; // ya reservada

            return _gymClassRepository.AddUserToClass(classId, userId);
        }

        // 🔹 CANCELAR RESERVA
        public bool CancelReservation(int classId, int userId)
        {
            var gymClass = _gymClassRepository.GetByIdWithUsers(classId);
            if (gymClass == null) return false;

            if (!gymClass.Users.Any(u => u.Id == userId))
                return true; // no había reserva

            return _gymClassRepository.RemoveUserFromClass(classId, userId);
        }

        // 🔹 ADMIN
        public GymClassResponse? CreateGymClass(CreateGymClassRequest request)
        {
            var gymClass = new GymClass
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                DuracionMinutos = request.DuracionMinutos,
                ImageUrl = request.ImageUrl,
                Dia = request.Dia,
                Hora = request.Hora,
                MaxCapacityUser = request.MaxCapacity
            };

            var created = _gymClassRepository.CreateGymClass(gymClass);
            if (created == null) return null;

            return new GymClassResponse
            {
                Id = created.Id,
                Nombre = created.Nombre,
                Descripcion = created.Descripcion,
                DuracionMinutos = created.DuracionMinutos,
                ImageUrl = created.ImageUrl,
                Dia = created.Dia,
                Hora = created.Hora,
                IsReservedByUser = false,
                MaxCapacity = created.MaxCapacityUser,
                CurrentEnrollments = 0
            };
        }

        // Application/Services/GymClassService.cs
        public bool UpdateGymClass(int id, UpdateGymClassRequest request)
        {
            var gymClass = _gymClassRepository.GetByIdWithUsers(id);  // ✅ Con usuarios para validar
            if (gymClass == null) return false;

            // ✅ VALIDACIÓN: No permitir reducir capacidad por debajo de inscriptos
            if (request.MaxCapacity < gymClass.Users.Count)
            {
                throw new InvalidOperationException(
                    $"No se puede reducir la capacidad a {request.MaxCapacity}. " +
                    $"Hay {gymClass.Users.Count} usuarios inscriptos. " +
                    $"La capacidad mínima debe ser {gymClass.Users.Count}."
                );
            }

            gymClass.Nombre = request.Nombre;
            gymClass.Dia = request.Dia;
            gymClass.Hora = request.Hora;
            gymClass.Descripcion = request.Descripcion;
            gymClass.DuracionMinutos = request.DuracionMinutos;
            gymClass.ImageUrl = request.ImageUrl;
            gymClass.MaxCapacityUser = request.MaxCapacity;  // ✅ AGREGAR

            return _gymClassRepository.UpdateGymClass(id, gymClass);
        }

        public bool DeleteGymClass(int id)
        {
            return _gymClassRepository.DeleteGymClass(id);
        }
        public GymClassDeleteSummaryResponse? GetDeleteSummary(int gymClassId)
        {
            var gymClass = _gymClassRepository.GetByIdWithUsers(gymClassId);
            if (gymClass == null) return null;

            var enrolledUsers = gymClass.Users.Select(u => $"{u.Nombre} {u.Apellido}".Trim()).ToList();
            var warnings = new List<string>();

            if (enrolledUsers.Count > 0)
            {
                warnings.Add($"⚠️ Se eliminarán {enrolledUsers.Count} inscripciones de usuarios");
                warnings.Add("⚠️ Los usuarios deberán reservar nuevamente si la clase se vuelve a crear");
            }

            var dayNames = new Dictionary<int, string>
    {
        { 1, "Lunes" }, { 2, "Martes" }, { 3, "Miércoles" },
        { 4, "Jueves" }, { 5, "Viernes" }, { 6, "Sábado" }, { 7, "Domingo" }
    };

            return new GymClassDeleteSummaryResponse
            {
                GymClassId = gymClass.Id,
                ClassName = gymClass.Nombre,
                DayAndTime = $"{dayNames[(int)gymClass.Dia]} - {gymClass.Hora}",
                EnrolledUsersCount = enrolledUsers.Count,
                EnrolledUsers = enrolledUsers,
                Warnings = warnings
            };
        }

        public GymClassEnrolledUsersResponse? GetEnrolledUsers(int gymClassId)
        {
            var gymClass = _gymClassRepository.GetByIdWithUsers(gymClassId);
            if (gymClass == null) return null;

            var dayNames = new Dictionary<int, string>
    {
        { 1, "Lunes" }, { 2, "Martes" }, { 3, "Miércoles" },
        { 4, "Jueves" }, { 5, "Viernes" }, { 6, "Sábado" }, { 7, "Domingo" }
    };

            return new GymClassEnrolledUsersResponse
            {
                GymClassId = gymClass.Id,
                ClassName = gymClass.Nombre,
                DayAndTime = $"{dayNames[(int)gymClass.Dia]} - {gymClass.Hora}",
                MaxCapacity = gymClass.MaxCapacityUser,
                CurrentEnrollments = gymClass.Users.Count,
                Users = gymClass.Users.Select(u => new EnrolledUserDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    PlanName = u.Subscriptions?
                        .Where(s => s.IsActive && s.EndDate > DateTime.Now)
                        .OrderByDescending(s => s.EndDate)
                        .FirstOrDefault()?.Plan?.Tipo.ToString() ?? "Sin Plan"
                }).ToList()
            };
        }
    }
}
