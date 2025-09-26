using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Historical
    {
        protected Historical()  { }
        public Historical(int id, int userId, DateTime date)
        {
            Id = id;
            UserId = userId;
            Fecha = date;
        }

        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime Fecha { get; set; }
    }
}

