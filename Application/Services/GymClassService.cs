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

        public bool DeleteGymClass(int id)
        {
            return _gymClassRepository.DeleteGymClass(id);
        }

        public List<GymClassResponse> GetAll()
        {
            var classes = _gymClassRepository.GetAll();
            return classes.Select(gc => new GymClassResponse
            {
                Id = gc.Id,
                Nombre = gc.Nombre,
                Descripcion = gc.Descripcion,
                DuracionMinutos = gc.DuracionMinutos,
                ImageUrl = gc.ImageUrl,
                Dia = gc.Dia,
                Hora = gc.Hora
            }).ToList();
        }

        public GymClassResponse? GetById(int id)
        {
            var gc = _gymClassRepository.GetById(id);
            if (gc == null) return null;

            return new GymClassResponse
            {
                Id = gc.Id,
                Nombre = gc.Nombre,
                Descripcion = gc.Descripcion,
                DuracionMinutos = gc.DuracionMinutos,
                ImageUrl = gc.ImageUrl,
                Dia = gc.Dia,
                Hora = gc.Hora
            };
        }

        public bool UpdateGymClass(int id, UpdateGymClassRequest request)
        {
            var gymClass = _gymClassRepository.GetById(id);
            if (gymClass == null) return false;

            gymClass.Nombre = request.Nombre;
            gymClass.Dia = request.Dia;
            gymClass.Hora = request.Hora;

            return _gymClassRepository.UpdateGymClass(id, gymClass);
        }

    }
}
