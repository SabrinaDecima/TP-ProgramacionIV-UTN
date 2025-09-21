using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserClass //tabla intermedia para relacion user y gymclass (muchos a muchos)
    {
        protected UserClass() { }
        public UserClass(int userId, int gymClassId)
        {
            UserId = userId;
            GymClassId = gymClassId;

        }
        public int UserId { get; set; }
        public User? User { get; set; } //relacion con usuario. 

        public int GymClassId { get; set; }
        public GymClass? GymClass { get; set; } //relacion con clase

    }
}

