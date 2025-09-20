

namespace Domain.Entities
{
    public class User 
    {
        public User(int id, string name, string lastName, string email, string phoneNumber, string password, Plan plan, Role role)
        {
            Id = id;
            Nombre = name;
            Apellido = lastName;
            Email = email;
            Telefono = phoneNumber;
            Contraseña = password; 
            Rol = role;
            Plan = plan;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Contraseña { get; set; }
        public Role Rol { get; set; }  // relacion con rol (1 a 1)
        public List<UserClass>? UserClasses { get; set; } // relacion con las clases del usuario (1 a muchos)
        public int PlanId { get; set; }
        public Plan Plan { get; set; } // relacion con plan (1 a 1)
        public List<Payment>? Pagos { get; set; } // Relacion con pagos (1 a muchos)

        public List<Reserve>? Reservas { get; set; } // Relacion con reservas (1 a muchos)

        public List<Historical>? Historiales { get; set; }// Relacion con historiales (1 a muchos)
    }
   
 }