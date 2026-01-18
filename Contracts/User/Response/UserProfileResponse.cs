namespace Contracts.User.Response
{
    public class UserProfileResponse : UserResponse
    {
        public int EnrolledClassesCount { get; set; }
        public List<GymClassSummary> EnrolledClasses { get; set; } = new();
    }

    public class GymClassSummary
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Dia { get; set; }
        public string Hora { get; set; } = string.Empty;
    }
}
