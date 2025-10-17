using Application.Abstraction;
using Application.Interfaces;
using Contracts.Enrollment.Request;
using Contracts.Enrollment.Response;
using Domain.Entities;
using System.Globalization;

namespace Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGymClassRepository _gymClassRepository;

        public EnrollmentService(
            IUserRepository userRepository,
            IGymClassRepository gymClassRepository)
        {
            _userRepository = userRepository;
            _gymClassRepository = gymClassRepository;
        }

        public bool EnrollUser(EnrollUserRequest request)
        {


            return _userRepository.EnrollUserToClass(request.UserId, request.GymClassId);
            
        }

        public bool UnenrollUser(EnrollUserRequest request)
        {

            return _userRepository.UnEnrollUserToClass(request.UserId, request.GymClassId);
        }


    }
}