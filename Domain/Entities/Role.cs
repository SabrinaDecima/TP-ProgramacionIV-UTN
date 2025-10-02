namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public Role() { }
        public string Nombre { get; set; }
        public List<User>? Users { get; set; } //ver (relacion con usuarios, 1 a muchos)
    }

    public enum TypeRole
    {
        Socio = 1,
        Administrador = 2,
        SuperAdministrador = 3
    }
}

