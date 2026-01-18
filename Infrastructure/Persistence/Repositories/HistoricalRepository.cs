using Application.Abstraction;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class HistoricalRepository : IHistoricalRepository
    {
        private readonly GymDbContext _context;

        public HistoricalRepository(GymDbContext context)
        {
            _context = context;
        }

        public void Add(Historical historical)
        {
            _context.Historicals.Add(historical);
            _context.SaveChanges();
        }

        public Historical? GetActive(int userId, int gymClassId)
        {
            return _context.Historicals
                    .FirstOrDefault(h =>
                    h.UserId == userId &&
                    h.GymClassId == gymClassId &&
                    h.Status == HistoricalStatus.Active)
                    ;
        }

        public void Update(Historical historical)
        {
            _context.Historicals.Update(historical);
            _context.SaveChanges();
        }

        public List<Historical> GetByUserId(int userId)
        {
            return _context.Historicals
                .Include(h => h.GymClass)
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.ActionDate)
                .ToList();
        }
    }
}
