using Application.Interfaces;
using Contracts.User.Request;
using Contracts.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        public bool CreateUser(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public List<UserResponse> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserResponse? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<UserResponse> Search(string? name, string? lastName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
