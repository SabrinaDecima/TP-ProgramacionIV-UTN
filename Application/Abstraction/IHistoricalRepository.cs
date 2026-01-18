using Domain.Entities;

namespace Application.Abstraction
{
    public interface IHistoricalRepository
    {
        void Add(Historical historical);
        Historical GetActive(int userId, int gymClassId);
        List<Historical> GetByUserId(int userId);
        void Update(Historical historical);


    }
}
