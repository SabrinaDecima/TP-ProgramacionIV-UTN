using Contracts.GymClass.Request;
using Contracts.GymClass.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGymClassService
    {
        List<GymClassResponse> GetAll();

        GymClassResponse? GetById(int id);

        bool CreateGymClass(CreateGymClassRequest request);

        bool UpdateGymClass(UpdateGymClassRequest request);

        bool DeleteGymClass(int id);
    }
}
