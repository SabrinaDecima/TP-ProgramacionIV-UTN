using Application.Abstraction;
using Application.Abstraction.ExternalService;
using Application.Interfaces;
using Contracts.User.Request;
using Contracts.User.Response;
using Domain.Entities;
using System.Data;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;
        


        public UserService(
            IUserRepository userRepository,
            IPlanRepository planRepository,
            IRoleRepository roleRepository,
            IPasswordService passwordService,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _planRepository = planRepository;
            _roleRepository = roleRepository;
            _passwordService = passwordService;
            _emailService = emailService;
        }

        public bool CreateUser(CreateUserRequest request)
        {
            
            if (string.IsNullOrWhiteSpace(request.Nombre) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Contraseña))
                return false;

            
            if (_userRepository.GetByEmail(request.Email) != null)
                return false;

            var plan = _planRepository.GetPlanById(request.PlanId);
            if (plan == null)
                return false;



            var defaultRole = _roleRepository.GetById((int)TypeRole.Socio);
            if (defaultRole == null)
                return false;

          

            var user = new User
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Email = request.Email,
                Telefono = request.Telefono,
                PlanId = request.PlanId,
                RoleId = defaultRole.Id,
                Plan = plan,
                Rol = defaultRole
            };

            user.Contraseña = _passwordService.HashPassword(user, request.Contraseña);

            return _userRepository.CreateUser(user);
        }

        //  Creación de usuario por Admin
        public bool CreateUserByAdmin(CreateUserByAdminRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre) ||
                string.IsNullOrWhiteSpace(request.Email))
                return false;

            if (_userRepository.GetByEmail(request.Email) != null)
                return false;

            var plan = _planRepository.GetPlanById(request.PlanId);
            if (plan == null)
                return false;

            var role = _roleRepository.GetById(request.RoleId);
            if (role == null)
                return false;
            var user = new User
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Email = request.Email,
                Telefono = request.Telefono,
                PlanId = request.PlanId,
                RoleId = role.Id,
                Plan = plan,
                Rol = role
            };

            
            user.Contraseña = _passwordService.HashPassword(user, request.Contraseña);


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
                    PlanId = u.PlanId ?? 0,
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
                PlanId = user.PlanId ?? 0,
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
                users = users.Where(u => u.Apellido.Contains(lastName, StringComparison.OrdinalIgnoreCase));
            }

            return users.Select(u => new UserResponse
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                Telefono = u.Telefono,
                PlanId = u.PlanId ?? 0,
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

        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null)
                return false;

            // Generación de token si el mail es válido

            var resetToken = Guid.NewGuid().ToString();
            user.PasswordResetToken = resetToken;
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(1);

            if (!_userRepository.UpdateUser(user.Id, user))
                return false;

            // Envio de email con el token

            var resetLink = $"http://localhost:4200/reset-password?token={resetToken}";
            var emailBody = $@"
                <p>Hola {user.Nombre},</p>
                <p>Para resetear tu contraseña, haz clic en el siguiente enlace:</p>
                <a href='{resetLink}'>Resetear Contraseña</a>
                <p>Este enlace expira en 1 hora.</p>";

            await _emailService.SendEmailAsync(user.Email, "Resetear Contraseña - FunctionFit", emailBody);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            token = token.Trim();

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(newPassword))
                return false;

            var users = _userRepository.GetUsers();
            var user = users.FirstOrDefault(u => u.PasswordResetToken == token);

            if (user == null || user.ResetTokenExpires < DateTime.UtcNow)
                return false;

            // Actualización de contraseña

            user.Contraseña = _passwordService.HashPassword(user, newPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            return _userRepository.UpdateUser(user.Id, user);
        }

         public async Task<User?> GetByEmailAsync(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        public UserProfileResponse? GetProfile(int userId)
        {
            var user = _userRepository.GetUserWithClasses(userId);
            if (user == null) return null;

            return new UserProfileResponse
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = user.Email,
                Telefono = user.Telefono,
                PlanId = user.PlanId ?? 0,
                EnrolledClassesCount = user.GymClasses?.Count ?? 0,
                EnrolledClasses = user.GymClasses?.Select(gc => new GymClassSummary
                {
                    Id = gc.Id,
                    Nombre = gc.Nombre,
                    Dia = (int)gc.Dia,
                    Hora = gc.Hora
                }).ToList() ?? new List<GymClassSummary>()
            };
        }

    }
}
