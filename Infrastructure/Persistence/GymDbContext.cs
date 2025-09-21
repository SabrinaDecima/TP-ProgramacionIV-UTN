using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options ) : base( options ) { }

        public DbSet<GymClass> GymClasses { get; set; }
        public DbSet<Historical> Historicals { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Reserve> Reserves  { get; set; }
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
        }


    }
}
