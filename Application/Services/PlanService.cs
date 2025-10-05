using Application.Abstraction;
using Application.Interfaces;
using Contracts.Plan.Request;
using Contracts.Plan.Response;
using Domain.Entities;
using System.Numerics;


namespace Application.Services.Implementations
{
    public class PlanService : IPlanService
    {

        private readonly IPlanRepository _planRepository;
        public PlanService(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public bool CreatePlan(CreatePlanRequest request)
        {
            if (!Enum.IsDefined(typeof(TypePlan), request.Tipo) || request.Precio < 0)
            {
                return false;
            }

            var existingPlans = _planRepository.GetAll();
            if (existingPlans.Any(p => p.Tipo == request.Tipo))
            {
                return false;
            }

            var newPlan = new Plan
            {
                Tipo = request.Tipo,
                Precio = request.Precio,
            };

            _planRepository.CreatePlan(newPlan);
            return true;
        }

        public bool UpdatePlan(UpdatePlanRequest request)
        {
            var plan = _planRepository.GetPlanById(request.Id);
            if (plan == null)
            {
                return false;
            }
            if (!Enum.IsDefined(typeof(TypePlan), request.Tipo) || request.Precio < 0)
            {
                return false;
            }
            plan.Tipo = request.Tipo;
            plan.Precio = request.Precio;
            return _planRepository.UpdatePlan(request.Id, plan);

        }

        public bool DeletePlan(int id)
        {
            return _planRepository.DeletePlan(id);
        }

        public List<PlanResponse> GetAll()
        {
            return _planRepository.GetAll()
                .Select(p => new PlanResponse
                {
                    Id = p.Id,
                    Tipo = p.Tipo,
                    Precio = p.Precio,
                }).ToList();
        }

        public PlanResponse? GetById(int id)
        {
            var plan = _planRepository.GetPlanById(id);
            if (plan == null) return null;
            return new PlanResponse
            {
                Id = plan.Id,
                Tipo = plan.Tipo,
                Precio = plan.Precio,
            };
        }

    }
}
