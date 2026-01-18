using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Seeding;



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
        public DbSet<Historical> Historicals { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de clases
            base.OnModelCreating(modelBuilder);

            // relación muchos-a-muchos SIN entidad intermedia
            modelBuilder.Entity<User>()
                .HasMany(u => u.GymClasses)
                .WithMany(gc => gc.Users)
                .UsingEntity(j => j
                    .ToTable("UserGymClass") 
                    .HasKey("UsersId", "GymClassesId") 
                );

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

            modelBuilder.Entity<Plan>()
              .Property(p => p.Precio)
              .HasPrecision(18, 2);


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

            modelBuilder.Entity<Historical>()
               .HasOne(h => h.User)
               .WithMany(u => u.Historicals)
               .HasForeignKey(h => h.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Historical>()
                .HasOne(h => h.GymClass)
                .WithMany(g => g.Historicals)
                .HasForeignKey(h => h.GymClassId)
                .OnDelete(DeleteBehavior.Cascade);

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
                new User { Id = 1, Nombre = "cliente", Apellido = "uno", Email = "cliente@demo.com", Telefono = "1234", Contraseña = PasswordSeeder.Hash("1234"), RoleId = (int)TypeRole.Socio, PlanId = (int)TypePlan.Basic },
                new User { Id = 2, Nombre = "admin", Apellido = "demo", Email = "admin@demo.com", Telefono = "5678", Contraseña = PasswordSeeder.Hash("1234"), RoleId = (int)TypeRole.Administrador, PlanId = null },   
                new User { Id = 3, Nombre = "superadmin", Apellido = "demo", Email = "superadmin@demo.com", Telefono = "9999", Contraseña = PasswordSeeder.Hash("1234"), RoleId = (int)TypeRole.SuperAdministrador, PlanId = null }
            );

            // Seed de Payments
            modelBuilder.Entity<Payment>().HasData(
                new Payment { Id = 1, UserId = 1, Monto = 25.0m, Fecha = "2025-10-10", Pagado = true },
                new Payment { Id = 2, UserId = 1, Monto = 25.0m, Fecha = "2025-11-10", Pagado = false },
                new Payment { Id = 3, UserId = 2, Monto = 45.0m, Fecha = "2025-10-10", Pagado = true }
            );

            // Seed de GymClass
            // Seed de GymClass
            modelBuilder.Entity<GymClass>().HasData(
                // === YOGA (capacidad = 3) ===
                new GymClass
                {
                    Id = 1,
                    Nombre = "Yoga",
                    Descripcion = "Clase de relajación y estiramiento",
                    DuracionMinutos = 60,
                    ImageUrl = "https://placehold.co/600x400?text=Yoga&font=montserrat",
                    Dia = DayOfTheWeek.Lunes,
                    Hora = "08:00",
                    MaxCapacityUser = 3
                },
                new GymClass
                {
                    Id = 3,
                    Nombre = "Yoga",
                    Descripcion = "Clase de relajación y estiramiento",
                    DuracionMinutos = 60,
                    ImageUrl = "https://placehold.co/600x400?text=Yoga&font=montserrat",
                    Dia = DayOfTheWeek.Miercoles,
                    Hora = "08:00",
                    MaxCapacityUser = 3
                },
                new GymClass
                {
                    Id = 5,
                    Nombre = "Yoga",
                    Descripcion = "Clase de relajación y estiramiento",
                    DuracionMinutos = 60,
                    ImageUrl = "https://placehold.co/600x400?text=Yoga&font=montserrat",
                    Dia = DayOfTheWeek.Viernes,
                    Hora = "08:00",
                    MaxCapacityUser = 3
                },

                // === CROSSFIT (capacidad = 3) ===
                new GymClass
                {
                    Id = 7,
                    Nombre = "CrossFit",
                    Descripcion = "Entrenamiento funcional de alta intensidad",
                    DuracionMinutos = 45,
                    ImageUrl = "https://placehold.co/600x400?text=CrossFit&font=montserrat",
                    Dia = DayOfTheWeek.Martes,
                    Hora = "07:00",
                    MaxCapacityUser = 3
                },
                new GymClass
                {
                    Id = 10,
                    Nombre = "CrossFit",
                    Descripcion = "Entrenamiento funcional de alta intensidad",
                    DuracionMinutos = 45,
                    ImageUrl = "https://placehold.co/600x400?text=CrossFit&font=montserrat",
                    Dia = DayOfTheWeek.Jueves,
                    Hora = "20:00",
                    MaxCapacityUser = 3
                },
                new GymClass
                {
                    Id = 12,
                    Nombre = "CrossFit",
                    Descripcion = "Entrenamiento funcional de alta intensidad",
                    DuracionMinutos = 45,
                    ImageUrl = "https://placehold.co/600x400?text=CrossFit&font=montserrat",
                    Dia = DayOfTheWeek.Sabado,
                    Hora = "11:00",
                    MaxCapacityUser = 3
                },

                // === SPINNING (capacidad = 15) ===
                new GymClass
                {
                    Id = 13,
                    Nombre = "Spinning",
                    Descripcion = "Ejercicio cardiovascular en bicicleta",
                    DuracionMinutos = 50,
                    ImageUrl = "https://placehold.co/600x400?text=Spinning&font=montserrat",
                    Dia = DayOfTheWeek.Lunes,
                    Hora = "20:00",
                    MaxCapacityUser = 15
                },
                new GymClass
                {
                    Id = 14,
                    Nombre = "Spinning",
                    Descripcion = "Ejercicio cardiovascular en bicicleta",
                    DuracionMinutos = 50,
                    ImageUrl = "https://placehold.co/600x400?text=Spinning&font=montserrat",
                    Dia = DayOfTheWeek.Martes,
                    Hora = "20:00",
                    MaxCapacityUser = 15
                },
                new GymClass
                {
                    Id = 15,
                    Nombre = "Spinning",
                    Descripcion = "Ejercicio cardiovascular en bicicleta",
                    DuracionMinutos = 50,
                    ImageUrl = "https://placehold.co/600x400?text=Spinning&font=montserrat",
                    Dia = DayOfTheWeek.Miercoles,
                    Hora = "20:00",
                    MaxCapacityUser = 15
                },

                // === PILATES (capacidad = 5) ===
                new GymClass
                {
                    Id = 19,
                    Nombre = "Pilates",
                    Descripcion = "Fortalecimiento y control postural",
                    DuracionMinutos = 55,
                    ImageUrl = "https://placehold.co/600x400?text=Pilates&font=montserrat",
                    Dia = DayOfTheWeek.Martes,
                    Hora = "18:00",
                    MaxCapacityUser = 5
                },
                new GymClass
                {
                    Id = 20,
                    Nombre = "Pilates",
                    Descripcion = "Fortalecimiento y control postural",
                    DuracionMinutos = 55,
                    ImageUrl = "https://placehold.co/600x400?text=Pilates&font=montserrat",
                    Dia = DayOfTheWeek.Jueves,
                    Hora = "18:00",
                    MaxCapacityUser = 5
                },
                new GymClass
                {
                    Id = 21,
                    Nombre = "Pilates",
                    Descripcion = "Fortalecimiento y control postural",
                    DuracionMinutos = 55,
                    ImageUrl = "https://placehold.co/600x400?text=Pilates&font=montserrat",
                    Dia = DayOfTheWeek.Sabado,
                    Hora = "10:00",
                    MaxCapacityUser = 5
                },

                // === BOXEO (capacidad = 8) ===
                new GymClass
                {
                    Id = 22,
                    Nombre = "Boxeo",
                    Descripcion = "Técnica, sacos y condición física",
                    DuracionMinutos = 60,
                    ImageUrl = "https://placehold.co/600x400?text=Boxeo&font=montserrat",
                    Dia = DayOfTheWeek.Lunes,
                    Hora = "19:00",
                    MaxCapacityUser = 8
                },
                new GymClass
                {
                    Id = 23,
                    Nombre = "Boxeo",
                    Descripcion = "Técnica, sacos y condición física",
                    DuracionMinutos = 60,
                    ImageUrl = "https://placehold.co/600x400?text=Boxeo&font=montserrat",
                    Dia = DayOfTheWeek.Miercoles,
                    Hora = "19:00",
                    MaxCapacityUser = 8
                },
                new GymClass
                {
                    Id = 24,
                    Nombre = "Boxeo",
                    Descripcion = "Técnica, sacos y condición física",
                    DuracionMinutos = 60,
                    ImageUrl = "https://placehold.co/600x400?text=Boxeo&font=montserrat",
                    Dia = DayOfTheWeek.Viernes,
                    Hora = "19:00",
                    MaxCapacityUser = 8
                },

                // === ZUMBA (capacidad = 20) ===
                new GymClass
                {
                    Id = 25,
                    Nombre = "Zumba",
                    Descripcion = "Baile y ritmo latino para quemar calorías",
                    DuracionMinutos = 50,
                    ImageUrl = "https://placehold.co/600x400?text=Zumba&font=montserrat",
                    Dia = DayOfTheWeek.Martes,
                    Hora = "20:30",
                    MaxCapacityUser = 20
                },
                new GymClass
                {
                    Id = 26,
                    Nombre = "Zumba",
                    Descripcion = "Baile y ritmo latino para quemar calorías",
                    DuracionMinutos = 50,
                    ImageUrl = "https://placehold.co/600x400?text=Zumba&font=montserrat",
                    Dia = DayOfTheWeek.Jueves,
                    Hora = "20:30",
                    MaxCapacityUser = 20
                },
                new GymClass
                {
                    Id = 27,
                    Nombre = "Zumba",
                    Descripcion = "Baile y ritmo latino para quemar calorías",
                    DuracionMinutos = 50,
                    ImageUrl = "https://placehold.co/600x400?text=Zumba&font=montserrat",
                    Dia = DayOfTheWeek.Sabado,
                    Hora = "12:00",
                    MaxCapacityUser = 20
                }
            );

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DefaultConnection", b => b.MigrationsAssembly("Infrastructure"));
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}


