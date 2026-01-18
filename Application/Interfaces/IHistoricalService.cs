

namespace Application.Interfaces
{
    public interface IHistoricalService
    {
        public List<HistoricalResponse> GetUserHistory(int userId);
    }
}
