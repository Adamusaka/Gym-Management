namespace backend.Entities;

public class GymVisitEntity {
    public Guid GymVisitId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public Guid UserId { get; set; }
}