
namespace Domain.Entities
{
    public class Instructor
    {
        public Instructor() { }
        public Instructor(int id, string name, string lastName)
        {
            Id = id;
            Nombre = name;
            Apellido = lastName;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public List<GymClass>? GymClasses { get; set; }   // Clases que dicta el instructor (1 a muchos)
    }
}

