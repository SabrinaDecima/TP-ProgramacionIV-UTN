using Contracts.User.Request;
using Contracts.User.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        List<UserResponse> GetAll();
        List<UserResponse> Search(string? name, string? lastName);

        UserResponse? GetById(int id);

        bool UpdateUser(int id, UpdateUserRequest request);

        bool DeleteUser(int id);
        bool ChangeUserPlan(int userId, int newPlanId);
        bool CreateUser(CreateUserRequest request);

        Task<bool> RequestPasswordResetAsync(string email);

        Task<bool> ResetPasswordAsync(string token, string newPassword);

        Task<User>? GetByEmailAsync(string email);
    }
}
