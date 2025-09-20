using Domain.Entities;


namespace Application.Abstraction
{
    public interface IUserRepository
    {
        List<User> GetUsers();

        User? GetById(int id);

        bool CreateUser(User user);

        bool UpdateUser(int id, User user);

        bool DeleteUser(int id);

        User? GetByEmail(string email);

        User? GetUserWithPlan(int id);

        User? GetUserWithClasses(int id);

        User? GetUserWithPayment(int id);

        bool ChangeUserRole(int id, string newRole);
    }
}
