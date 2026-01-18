using Domain.Entities;

public class Historical
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public int GymClassId { get; set; }

    public DateTime ClassDate { get; set; }
    public DateTime ActionDate { get; set; }

    public HistoricalStatus Status { get; set; }

    public GymClass GymClass { get; set; }
    public User User { get; set; }
}


public enum HistoricalStatus
{
    Active,
    Cancelled,
    Completed
}

