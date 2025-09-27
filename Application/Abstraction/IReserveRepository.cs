using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Abstraction
{
    public interface IReservaRepository
    {
        List<Reserve> GetAll();
        Reserve GetById(int id);
        Reserve CreateReserve(Reserve reserva);
        bool UpdateReserve(Reserve reserva);
        bool DeleteReseve(int id);

        List<Reserve> GetReservasByUser(int userId);
    }
}

