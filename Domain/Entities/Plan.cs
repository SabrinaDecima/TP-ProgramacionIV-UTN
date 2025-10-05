
namespace Domain.Entities
{
    public class Plan : BaseEntity
    {
        public Plan() { }
        public Plan(int id, TypePlan tipo, decimal price)
        {
            Id = id;
            Tipo= tipo;
            Precio = price;
        }


        public int Id { get; set; }
        public TypePlan Tipo { get; set; }

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
