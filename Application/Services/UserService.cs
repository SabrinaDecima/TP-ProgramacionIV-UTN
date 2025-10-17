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
            
            if (string.IsNullOrWhiteSpace(request.Nombre) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Contraseña))
                return false;

            
            if (_userRepository.GetByEmail(request.Email) != null)
                return false;

            
            var role = _roleRepository.GetById(request.RoleId);
            if (role == null)
                return false;

            
            Plan? plan = null;
            if (request.RoleId == (int)TypeRole.Socio && request.PlanId.HasValue)
            {
                plan = _planRepository.GetPlanById(request.PlanId);
                if (plan == null)
                    return false;
            }
            else if (request.RoleId == (int)TypeRole.Socio && !request.PlanId.HasValue)
            {
                return false; 
            }
            

            var user = new User
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Email = request.Email,
                Telefono = request.Telefono,
                Contraseña = request.Contraseña,
                PlanId = request.PlanId,
                RoleId = request.RoleId,
                Plan = plan,
                Rol = role
            };

            return _userRepository.CreateUser(user);
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
                    PlanId = (int)u.PlanId,
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
                PlanId = (int)user.PlanId,
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
                PlanId = (int)u.PlanId
            }).ToList();
        }

        public bool UpdateUser(int id, UpdateUserRequest request)
        {
            
            if (string.IsNullOrWhiteSpace(request.Nombre) ||
                string.IsNullOrWhiteSpace(request.Email))
                return false;

            
            var existingUser = _userRepository.GetById(id);
            if (existingUser == null)
                return false;

            
            var plan = _planRepository.GetPlanById(request.PlanId);
            if (plan == null)
                return false;

          
            existingUser.Nombre = request.Nombre;
            existingUser.Apellido = request.Apellido;
            existingUser.Email = request.Email;
            existingUser.Telefono = request.Telefono;
            existingUser.PlanId = request.PlanId;
            existingUser.Plan = plan; 

            
            return _userRepository.UpdateUser(id, existingUser);
        }



        public bool ChangeUserPlan(int userId, int newPlanId)
        {
            
            var newPlan = _planRepository.GetPlanById(newPlanId);
            if (newPlan == null) return false;

            
            var user = _userRepository.GetById(userId);
            if (user == null) return false;

           
            if (user.PlanId == newPlanId) return true;

           
            user.PlanId = newPlanId;
            user.Plan = newPlan;

            return _userRepository.UpdateUser(userId, user);
        }
    }
}
