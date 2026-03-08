namespace Contracts.GymClass.Response
{
    public class GymClassDeleteSummaryResponse
    {
        public int GymClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string DayAndTime { get; set; } = string.Empty;
        public int EnrolledUsersCount { get; set; }
        public List<string> EnrolledUsers { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
    }
}