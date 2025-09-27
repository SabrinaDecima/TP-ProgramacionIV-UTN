using Domain.Entities;


namespace Domain.Abstraction
{
    public interface IHistoricalRepository
    {
        List<Historical> GetAll();
        Historical GetById(int id);

        Historical CreateHistorical(Historical historical);
        bool DeleteHistorical(int id);
        bool UpdateHistorical(int id, Historical historical);
        List<Historical> GetByUser(int userId);
    }
}
