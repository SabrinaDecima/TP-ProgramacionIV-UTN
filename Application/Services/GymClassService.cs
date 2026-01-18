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
        public bool CreateGymClass(CreateGymClassRequest request)
        {
            var gymClass = new GymClass
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                DuracionMinutos = request.DuracionMinutos,
                ImageUrl = request.ImageUrl,
                Dia = request.Dia,
                Hora = request.Hora
            };

            var created = _gymClassRepository.CreateGymClass(gymClass);
            return created != null;
        }

        public bool UpdateGymClass(int id, UpdateGymClassRequest request)
        {
            var gymClass = _gymClassRepository.GetById(id);
            if (gymClass == null) return false;

            gymClass.Nombre = request.Nombre;
            gymClass.Dia = request.Dia;
            gymClass.Hora = request.Hora;
            gymClass.Descripcion = request.Descripcion;
            gymClass.DuracionMinutos = request.DuracionMinutos;
            gymClass.ImageUrl = request.ImageUrl;

            return _gymClassRepository.UpdateGymClass(id, gymClass);
        }

        public bool DeleteGymClass(int id)
        {
            return _gymClassRepository.DeleteGymClass(id);
        }
    }
}
