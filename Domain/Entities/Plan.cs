
namespace Domain.Entities
{
    public class Plan
    {
        public Plan(int id, string name)
        {
            Id = id;
            Nombre = name;
        }

        public int Id { get; set; }
        public string Nombre { get; set; } // tipo de plan

        public List<User>? Users { get; set; } // relacion 1 a muchos con User
    }

}
