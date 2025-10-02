using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options ) : base( options ) { }

        public DbSet<GymClass> GymClasses { get; set; }  
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Role > Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClass > UserClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserClass>()
                .HasKey(uc => new { uc.UserId, uc.GymClassId });

            modelBuilder.Entity<UserClass>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserClasses)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserClass>()
                .HasOne(uc => uc.GymClass)
                .WithMany(gc => gc.UserClasses)
                .HasForeignKey(uc => uc.GymClassId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Rol) 
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Plan>()
                .HasMany(p => p.Users)
                .WithOne(u => u.Plan) 
                .HasForeignKey(u => u.PlanId);

            modelBuilder.Entity<Plan>().HasData(
            new Plan { Id = 1, Nombre = nameof(TypePlan.Basic), Precio = 25.5m},
            new Plan { Id = 2, Nombre = nameof(TypePlan.Premium), Precio = 45.0m},
            new Plan { Id = 3, Nombre = nameof(TypePlan.Elite), Precio = 70.0m }
            );


            modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Nombre = "cliente", Apellido = "uno", Email = "cliente@demo.com", Telefono = "1234", Contraseña = "1234", RoleId = (int)TypeRole.Socio, PlanId = (int)TypePlan.Basic },
            new User { Id = 2, Nombre = "admin", Apellido = "demo", Email = "admin@demo.com", Telefono = "5678", Contraseña = "1234", RoleId = (int)TypeRole.Administrador, PlanId = (int)TypePlan.Premium },
            new User { Id = 3, Nombre = "superadmin", Apellido = "demo", Email = "superadmin@demo.com", Telefono = "9999", Contraseña = "1234", RoleId = (int)TypeRole.SuperAdministrador, PlanId = (int)TypePlan.Elite }
);

            // Seed de datos con los enums de Role
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Nombre = nameof(TypeRole.Socio) },
                new Role { Id = 2, Nombre = nameof(TypeRole.Administrador) },
                new Role { Id = 3, Nombre = nameof(TypeRole.SuperAdministrador) });

            
            modelBuilder.Entity<GymClass>()
                .HasOne(gc => gc.Instructor)
                .WithMany() 
                .HasForeignKey(gc => gc.InstructorId)
                .OnDelete(DeleteBehavior.Restrict); // Evita borrado en cascada
        }

    
    }
}
