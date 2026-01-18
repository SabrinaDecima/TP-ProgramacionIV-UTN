public class HistoricalResponse
{
    public int GymClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime ClassDate { get; set; }
    public DateTime ActionDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
