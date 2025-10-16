﻿using Application.Abstraction;
using Contracts.Login.Request;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        private readonly GymDbContext _context;

        public UserRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }
        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }
        
        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChanges() > 0;

        }

        public bool UpdateUser(int id, User user)
        {
            _context.Users.Update(user);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteUser (int id)
        {
            var user = _context.Users.FirstOrDefault(u =>u.Id == id);
            if (user == null) 
                return false;
            _context.Users.Remove(user);
            return _context.SaveChanges() > 0;

        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User? GetByEmailAndPassword (LoginRequest request)
        {
            return _context.Users
                .Include( x => x.Rol)
                .FirstOrDefault( x => x.Email == request.Email && x.Contraseña == request.Password);
        }


        public User? GetUserWithPlan(int id)
        {
            return _context.Users
                .Include(u => u.Plan)
                .FirstOrDefault(u => u.Id == id);
        }

        public User? GetUserWithClasses(int id)
        {
            return _context.Users
                .Include(u => u.GymClasses)
                .FirstOrDefault(u => u.Id == id);
        }

        public User? GetUserWithPayment(int id)
        {
            return _context.Users
                .Include(u => u.Pagos)
                .FirstOrDefault(u => u.Id == id);
        }

        public bool ChangeUserRole(int id, string newRole)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return false; // Usuario no encontrado
            }
            var role = _context.Roles.FirstOrDefault(r => r.Nombre == newRole);
            if (role == null)
            {
                return false; // Rol no encontrado
            }
            user.RoleId = role.Id;
            user.Rol = role;
            _context.Users.Update(user);
            return _context.SaveChanges() > 0;
        }

        public User? GetUserWithClassesAndPayments(int id)
        {
            return _context.Users
                .Include(u => u.GymClasses)
                .Include(u => u.Pagos)
                .FirstOrDefault(u => u.Id == id);
        }

        public bool EnrollUserToClass(int userId, int gymClassId)
        {
            var user = _context.Users
                .Include(u => u.GymClasses)
                .FirstOrDefault(u => u.Id == userId);

            var gymClass = _context.GymClasses
                .Include(g => g.Users)
                .FirstOrDefault(g => g.Id == gymClassId);

            if (user == null || gymClass == null)
                return false;

            user.GymClasses ??= new List<GymClass>();
            gymClass.Users ??= new List<User>();

            if (!user.GymClasses.Contains(gymClass))
                user.GymClasses.Add(gymClass);
            if (!gymClass.Users.Contains(user))
                gymClass.Users.Add(user);

            return _context.SaveChanges() > 0;

        }

       
    }
}