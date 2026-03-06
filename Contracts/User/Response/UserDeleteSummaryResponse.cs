namespace Contracts.User.Response
{
    public class UserDeleteSummaryResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public bool HasActiveSubscription { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public int EnrolledClassesCount { get; set; }
        public int PaymentsCount { get; set; }
        public List<string> Warnings { get; set; } = new();
    }
}
