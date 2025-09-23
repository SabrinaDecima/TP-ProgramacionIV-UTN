
namespace Domain.Entities
{
    public class UserClass //tabla intermedia para relacion user y gymclass (muchos a muchos)
    {
        public UserClass(int userId, int classId)
        {
            UserId = userId;
            ClaseId = classId;
        }
        public int UserId { get; set; }
        public User? User { get; set; } //relacion con usuario. 

        public int ClaseId { get; set; }
        public GymClass? Clase { get; set; } //relacion con clase

    }
}

