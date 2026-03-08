namespace Contracts.GymClass.Response
{
    public class GymClassEnrolledUsersResponse
    {
        public int GymClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string DayAndTime { get; set; } = string.Empty;
        public int MaxCapacity { get; set; }
        public int CurrentEnrollments { get; set; }
        public List<EnrolledUserDto> Users { get; set; } = new();
    }

    public class EnrolledUserDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
    }
}