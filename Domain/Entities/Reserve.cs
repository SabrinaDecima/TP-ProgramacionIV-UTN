

namespace Domain.Entities
{
    public class Reserve
    {
        protected Reserve() { }
        public Reserve(int id, int userId, int gymClassId, DateTime dateReserve)
        {
            Id = id;
            UserId = userId;
            GymClassId = gymClassId;
            FechaReserva = dateReserve;
        }

        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; } //relacion con usuario (1 a 1).

        public int GymClassId { get; set; }
        public GymClass? GymClass { get; set; } //relacion con clase (1 a 1).

        public DateTime FechaReserva { get; set; }
        public bool Asistio { get; set; } = false;
        public bool Pagado { get; set; } = false;
    }
}

