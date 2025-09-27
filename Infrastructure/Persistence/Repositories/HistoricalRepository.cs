
using Domain.Abstraction;
using Domain.Entities;
using Infrastructure.Persistence;
namespace Infrastructure.Repositories
{
    public class HistoricalRepository : IHistoricalRepository
    {
        private readonly GymDbContext _context;

        public HistoricalRepository(GymDbContext context)
        {
            _context = context;
        }

        public List<Historical> GetAll() {
            return _context.Historicals.ToList();
        }

        public Historical GetById(int id)
        {
            return _context.Historicals.Find(id);
        }

        public Historical CreateHistorical(Historical historical)
        {
            _context.Historicals.Add(historical);
            _context.SaveChanges();
            return historical;
        }

        public bool UpdateHistorical(int id, Historical historical)
        {
            _context.Historicals.Update(historical);
            return _context.SaveChanges() > 0;
        }
        public bool DeleteHistorical(int id)
        {
            var entity = _context.Historicals.Find(id);
            if (entity != null)
            {
                _context.Historicals.Remove(entity);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public List<Historical> GetByUser(int userId) =>
            _context.Historicals.Where(h => h.UserId == userId).ToList();
    }
}
