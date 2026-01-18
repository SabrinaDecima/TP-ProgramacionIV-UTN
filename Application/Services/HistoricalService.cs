using Application.Abstraction;
using Application.Interfaces;


namespace Application.Services
{
    public class HistoricalService : IHistoricalService
    {
        private readonly IHistoricalRepository _historicalRepository;

        public HistoricalService(IHistoricalRepository historicalRepository)
        {
            _historicalRepository = historicalRepository;
        }

        public List<HistoricalResponse> GetUserHistory(int userId)
        {
            return _historicalRepository.GetByUserId(userId)
                .Select(h => new HistoricalResponse
                {
                    GymClassId = h.GymClassId,
                    ClassName = h.GymClass?.Nombre ?? "", // en caso de null
                    ImageUrl = h.GymClass?.ImageUrl ?? "",
                    ClassDate = h.ClassDate,
                    ActionDate = h.ActionDate,
                    Status = h.Status.ToString()
                })
                .ToList();
        }

    }
}
