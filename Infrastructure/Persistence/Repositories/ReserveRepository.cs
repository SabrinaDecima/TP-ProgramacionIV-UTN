using Domain.Entities;
using Domain.Abstraction;
using Infrastructure.Persistence;


namespace Infrastructure.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly GymDbContext _context;

        public ReservaRepository(GymDbContext context)
        {
            _context = context;
        }

        public List<Reserve> GetAll() {
            return _context.Reserves.ToList();
        }


        public Reserve? GetById(int id)
        {
            return _context.Reserves.Find(id);
        }

        public Reserve CreateReserve(Reserve reserve)
        {
            _context.Reserves.Add(reserve);
            _context.SaveChanges();
            return reserve;
        }

        public bool UpdateReserve(Reserve reserva)
        {
            _context.Reserves.Update(reserva);
            var result = _context.SaveChanges();
            return result > 0;
        }

        public bool DeleteReseve(int id)
        {
            var entity = _context.Reserves.Find(id);
            if (entity != null)
            {
                _context.Reserves.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Reserve> GetReservasByUser(int userId) =>
            _context.Reserves.Where(r => r.UserId == userId).ToList();
    }
}
