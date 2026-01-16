namespace Contracts.Enrollment.Response
{
    public class EnrollmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? GymClassId { get; set; }          
        public bool? IsReserved { get; set; }       
    }
}