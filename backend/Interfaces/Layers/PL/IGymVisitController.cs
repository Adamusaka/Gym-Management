namespace backend.Interfaces.Layers.PL;

public class CheckInBody {
    public Guid UserId { get; set; }
}

public class CheckOutBody {
    public Guid GymVisitId { get; set; }
}