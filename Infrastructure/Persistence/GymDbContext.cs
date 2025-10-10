using Domain.Entities;
using Microsoft.EntityFrameworkCore;



namespace Infrastructure.Persistence
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options ) : base( options ) { }

        public DbSet<GymClass> GymClasses { get; set; }  
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Role > Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClass > UserClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de UserClass
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

            // Configuración de Role
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.RoleId);

            // Configuración de Plan
            modelBuilder.Entity<Plan>()
                .HasMany(p => p.Users)
                .WithOne(u => u.Plan)
                .HasForeignKey(u => u.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de Payment
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Pagos) 
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Monto)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Fecha)
                .HasMaxLength(50);

            // Seed de Planes
            modelBuilder.Entity<Plan>().HasData(
                new Plan { Id = 1, Tipo = TypePlan.Basic, Precio = 25.0m },
                new Plan { Id = 2, Tipo = TypePlan.Premium, Precio = 45.0m },
                new Plan { Id = 3, Tipo = TypePlan.Elite, Precio = 70.0m }
            );

            // Seed de Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Nombre = nameof(TypeRole.Socio) },
                new Role { Id = 2, Nombre = nameof(TypeRole.Administrador) },
                new Role { Id = 3, Nombre = nameof(TypeRole.SuperAdministrador) }
            );

            // Seed de Usuarios
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Nombre = "cliente", Apellido = "uno", Email = "cliente@demo.com", Telefono = "1234", Contraseña = "1234", RoleId = (int)TypeRole.Socio, PlanId = (int)TypePlan.Basic },
                new User { Id = 2, Nombre = "admin", Apellido = "demo", Email = "admin@demo.com", Telefono = "5678", Contraseña = "1234", RoleId = (int)TypeRole.Administrador, PlanId = (int)TypePlan.Premium },
                new User { Id = 3, Nombre = "superadmin", Apellido = "demo", Email = "superadmin@demo.com", Telefono = "9999", Contraseña = "1234", RoleId = (int)TypeRole.SuperAdministrador, PlanId = (int)TypePlan.Elite }
            );

            // Seed de Payments
            modelBuilder.Entity<Payment>().HasData(
                new Payment { Id = 1, UserId = 1, Monto = 25.0m, Fecha = "2025-10-10", Pagado = true },
                new Payment { Id = 2, UserId = 1, Monto = 25.0m, Fecha = "2025-11-10", Pagado = false },
                new Payment { Id = 3, UserId = 2, Monto = 45.0m, Fecha = "2025-10-10", Pagado = true }
            );
        }



    }
}
