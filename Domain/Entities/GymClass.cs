

using Domain.Entities;

namespace Domain.Entities
{
    public class GymClass
    {
        public GymClass() { }
        public GymClass(int id, string name, string description, int durationMinute, string imageUrl, string date, string hour)
        {
            Id = id;
            Nombre = name;
            Descripcion = description;
            DuracionMinutos = durationMinute;
            ImageUrl = imageUrl;
            Fecha = date;
            Hora = hour;

        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int DuracionMinutos { get; set; }
        public string ImageUrl { get; set; }

        public string Fecha { get; set; }

        public string Hora { get; set; }

        public int MaxCapacityUser { get; set; } = 3;
        public List<User> Users { get; set; } = new(); //relcion con usuarios 

    }
}




