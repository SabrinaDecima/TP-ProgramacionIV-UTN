using Domain.Entities;

namespace Application.Abstraction
{
    public interface IRoleRepository
    {
        Role? GetById(int id);
        List<Role> GetAll();
    }
}