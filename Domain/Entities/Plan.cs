
namespace Domain.Entities
{
    public class Plan : BaseEntity
    {
        public Plan() { }
        public Plan(int id, string name, decimal price)
        {
            Id = id;
            Nombre = name;
            Precio = price;
        }


        public int Id { get; set; }
        public string Nombre { get; set; } // tipo de plan

        public decimal Precio { get; set; }

        public List<User>? Users { get; set; } // relacion 1 a muchos con User
    }

    public enum TypePlan
    {
        Basic = 1,
        Premium = 2,
        Elite = 3
    }

}
