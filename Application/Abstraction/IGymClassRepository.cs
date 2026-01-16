using Domain.Entities;

namespace Application.Abstraction
{
    public interface IGymClassRepository
    {
        List<GymClass> GetAll();
        List<GymClass> GetAllWithUsers();

        GymClass? GetById(int id);
        GymClass? GetByIdWithUsers(int id);

        GymClass CreateGymClass(GymClass gymClass);
        bool UpdateGymClass(int id, GymClass gymClass);
        bool DeleteGymClass(int id);

        bool AddUserToClass(int classId, int userId);
        bool RemoveUserFromClass(int classId, int userId);
    }
}
