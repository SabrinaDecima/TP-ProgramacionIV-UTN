using Contracts.GymClass.Request;
using Contracts.GymClass.Response;

namespace Application.Interfaces
{
    public interface IGymClassService
    {

        List<GymClassResponse> GetAll(int userId);


        GymClassResponse? GetById(int id, int userId);


        bool ReserveClass(int classId, int userId);
        bool CancelReservation(int classId, int userId);


        bool CreateGymClass(CreateGymClassRequest request);
        bool UpdateGymClass(int id, UpdateGymClassRequest request);
        bool DeleteGymClass(int id);
    }
}
