

namespace Domain.Entities
{
    public class User : BaseEntity
    {

        public User()
        {
        }
        public User(int id, string name, string lastName, string email, string phoneNumber, string password, Role role, Plan? plan = null )
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

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Contraseña { get; set; }

        public int RoleId { get; set; }
        public Role Rol { get; set; }  // relacion con rol (1 a 1)
        
        public List<GymClass>? GymClasses { get; set; } // relacion con las clases 
        public int? PlanId { get; set; }
        public Plan? Plan { get; set; } // relacion con plan (1 a 1)

        public List<Payment>? Pagos { get; set; } // Relacion con pagos (1 a muchos)

    }
   
 }