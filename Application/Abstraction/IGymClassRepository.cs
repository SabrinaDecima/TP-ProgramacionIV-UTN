using Domain.Entities;


namespace Application.Abstraction
{
    public interface IGymClassRepository
    {
        List<GymClass> GetAll();

        GymClass? GetById(int id);

        GymClass CreateGymClass (GymClass gymClass);

        bool UpdateGymClass (int id, GymClass gymClass);

        bool DeleteGymClass (int id);
    }
}
