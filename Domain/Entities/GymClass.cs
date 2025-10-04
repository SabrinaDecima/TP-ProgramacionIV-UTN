

using Domain.Entities;

namespace Domain.Entities
{
    public class GymClass
    {
        public GymClass() { }
        public GymClass(int id, string name, string description,string instructor, int durationMinute, string imageUrl)
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
        public string Instructor { get; set; }
        public int DuracionMinutos { get; set; }
        public string ImageUrl { get; set; }

        public List<UserClass> UserClasses { get; set; } = new(); //relcion con usuarios (1 a muchos)
        public DateTime Fecha { get; set; }

    }
}




