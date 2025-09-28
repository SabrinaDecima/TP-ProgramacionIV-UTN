using Contracts.User.Request;
using Contracts.User.Response;
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

        bool CreateUser(CreateUserRequest request);

        bool UpdateUser(int id, UpdateUserRequest request);

        bool DeleteUser(int id);
    }
}
