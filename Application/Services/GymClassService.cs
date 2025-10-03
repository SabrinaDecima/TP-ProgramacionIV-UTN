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
                Fecha = request.Fecha
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
                Fecha = gc.Fecha
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
                Fecha = gc.Fecha
            };
        }

        public bool UpdateGymClass(UpdateGymClassRequest request)
        {
            var gymClass = _gymClassRepository.GetById(request.Id);
            if (gymClass == null) return false;

            gymClass.Nombre = request.Nombre;
            gymClass.Fecha = request.Fecha;


            return _gymClassRepository.UpdateGymClass(request.Id, gymClass);
        }

    }
}
