using Contracts.Login.Request;
using Domain.Entities;

namespace Application.Abstraction
{
    public interface IUserRepository : IBaseRepository<User>
    {
        List<User> GetUsers();
        User? GetById(int id);
        bool CreateUser(User user);
        bool UpdateUser(int id, User user);
        bool DeleteUser(int id);
        User? GetByEmailAndPassword(LoginRequest request);
        User? GetByEmail(string email);
        User? GetUserWithClasses(int id);
        User? GetUserWithPayment(int id);
        User? GetUserWithSubscriptions(int id);
        bool ChangeUserRole(int id, string newRole);
        User? GetUserWithClassesAndPayments(int id);
    }
}
