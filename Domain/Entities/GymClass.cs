

using Domain.Entities;

namespace Domain.Entities
{
    public class GymClass
    {
        public GymClass(int id, string name, string description, Instructor instructor, int durationMinute, string imageUrl)
        {
            Id = id;
            Nombre = name;
            Descripcion = description;
            Instructor = instructor;
            DuracionMinutos = durationMinute;
            ImageUrl = imageUrl;
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; } // Relacion con Instructor (1 a 1)
        public int DuracionMinutos { get; set; }
        public string ImageUrl { get; set; }

        public List<UserClass>? UserClasses { get; set; } //relcion con usuarios (1 a muchos)
    }
}




