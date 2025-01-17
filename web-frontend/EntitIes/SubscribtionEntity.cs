namespace web_frontend.Entities;

public class SubscribtionEntity {
    public Guid SubscribtionId { get; set; }
    public string Type { get; set; }
    public string JoinDate { get; set; }
    public string EndDate { get; set; }
    public Guid UserId { get; set; }
}