

using Domain.Entities;

namespace Domain.Entities
{
    public class GymClass
    {
        public GymClass() { }
        public GymClass(int id, string name, string description, int durationMinute, string imageUrl, DayOfTheWeek day, string hour)
        {
            Id = id;
            Nombre = name;
            Descripcion = description;
            DuracionMinutos = durationMinute;
            ImageUrl = imageUrl;
            Dia = day;
            Hora = hour;

        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int DuracionMinutos { get; set; }
        public string ImageUrl { get; set; }

        public DayOfTheWeek Dia{ get; set; }

        public string Hora { get; set; }

        public int MaxCapacityUser { get; set; } = 3;
        public List<User> Users { get; set; } = new(); //relcion con usuarios 
        public List<Historical> Historicals { get; set; } = new();

    }

    public enum DayOfTheWeek
    {
        Lunes = 1,
        Martes = 2,
        Miercoles = 3,
        Jueves = 4,
        Viernes = 5,
        Sabado = 6,
        Domingo = 7
    }
}




