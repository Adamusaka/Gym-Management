namespace backend.Entities;

public class SubscriptionEntity {
    public Guid SubscribtionId { get; set; }
    public string? JoinDate { get; set; } = null;
    public string? EndDate { get; set; } = null;
    public Guid UserId { get; set; }
}