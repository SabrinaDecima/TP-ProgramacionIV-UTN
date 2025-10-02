using Application.Abstraction;
using Application.Interfaces;
using Contracts.User.Request;
using Contracts.User.Response;
using Domain.Entities;
using System;
using System.Data;



namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(
            IUserRepository userRepository,
            IPlanRepository planRepository,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _planRepository = planRepository;
            _roleRepository = roleRepository;
        }

        public bool CreateUser(CreateUserRequest request)
        {
            var existingUser = _userRepository.GetByEmail(request.Email);
            if (existingUser != null)
            {
                return false; // usuario ya existe
            }

            // Obtener Plan real desde la base de datos
            var plan = _planRepository.GetPlanById(request.PlanId);
            if (plan == null)
            {
                return false; // plan no encontrado
            }

            // Obtener Role real desde la base de datos
            var role = _roleRepository.GetById(request.RoleId);
            if (role == null)
            {
                return false; // rol no encontrado
            }

            var newUser = new User(
                0,
                request.Nombre,
                request.Apellido,
                request.Email,
                request.Telefono,
                request.Contraseña,
                plan,
                role
            );

            return _userRepository.CreateUser(newUser);
        }

        public bool DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);

            if(user == null)
            {
              return false ;
            }
            return _userRepository.DeleteUser(user.Id);
        }

        public List<UserResponse> GetAll()
        {
            var userList = _userRepository
                .GetUsers()
                .Select( u => new UserResponse
                { 
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    Telefono = u.Telefono,
                    PlanId = u.PlanId,
               }).ToList();

            return userList;
        }

        public UserResponse? GetById(int id)
        {
            var user = _userRepository.GetById(id);

            if(user == null) { return null; }

            return new UserResponse
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = user.Email,
                Telefono = user.Telefono,
                PlanId = user.PlanId,
            };
        }

        public List<UserResponse> Search(string? name, string? lastName)
        {
            var users = _userRepository.GetUsers().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                users = users.Where(u => u.Nombre.Contains(name, StringComparison.OrdinalIgnoreCase));
            }


            if (!string.IsNullOrWhiteSpace(lastName))
            {
                users = users.Where(u => u.Nombre.Contains(lastName, StringComparison.OrdinalIgnoreCase));
            }

            return users.Select(u => new UserResponse
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                Telefono = u.Telefono,
                PlanId = u.PlanId
            }).ToList();
        }

        public bool UpdateUser(int id, UpdateUserRequest request)
        {
            var existingUser = _userRepository.GetById(id);

            if(existingUser == null)
            {
                return false;
            }

            if(string.IsNullOrWhiteSpace(request.Nombre) || string.IsNullOrWhiteSpace(request.Email))
            {
                return false;
            }

            existingUser.Nombre = request.Nombre;
            existingUser.Apellido = request.Apellido;
            existingUser.Email = request.Email;
            existingUser.Telefono = request.Telefono;
            existingUser.PlanId = request.PlanId;

            return _userRepository.UpdateUser(id, existingUser);
        }
    }
}
