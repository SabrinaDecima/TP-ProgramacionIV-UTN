

namespace Domain.Entities
{
    public class Role
    {
        public Role(int id, string name) 
        {
            Id = id;
            Nombre = name;
        }
        public int Id { get; set; }
        public string Nombre { get; set; }

        public List<User>? Users { get; set; } //ver (relacion con usuarios, 1 a muchos)
    }
}

