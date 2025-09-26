
namespace Domain.Entities
{
    public class UserClass //tabla intermedia para relacion user y gymclass (muchos a muchos)
    {
        protected UserClass() { }
        public UserClass(int userId, int gymClassId)
        {
            UserId = userId;
<<<<<<< HEAD
            ClaseId = classId;
=======
            GymClassId = gymClassId;

>>>>>>> 61ef88e1501b0121f52252eb0c756e621c584e30
        }
        public int UserId { get; set; }
        public User? User { get; set; } //relacion con usuario. 

        public int GymClassId { get; set; }
        public GymClass? GymClass { get; set; } //relacion con clase

    }
}

