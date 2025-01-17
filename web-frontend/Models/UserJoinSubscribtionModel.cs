namespace web_frontend.Models;

public class UserJoinSubscriptionModel {
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; } = "Customer";
    public string? JoinDate { get; set; } = null;
    public string? EndDate { get; set; } = null;
    public bool IsSubscribed { get; set; } = false;
}