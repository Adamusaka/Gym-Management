using backend.Entities;

namespace backend.Interfaces.Layers.BLL;

public interface IGymVisitLogic
{
    public Task<ObtainGymVisitOutputData> ObtainGymVisit();
    public Task<CheckInOutputData> CheckIn(CheckInInputData checkInInputData);
    public Task<CheckOutOutputData> CheckOut(CheckOutInputData checkOutInputData);
}

public class ObtainGymVisitOutputData
{
    public bool isError { get; set; }
    public List<GymVisitEntity>? gymVisits { get; set; }
}


public class CheckInInputData
{
    public Guid UserId { get; set; }
}

public class CheckInOutputData
{
    public bool isError { get; set; }
    public bool? isValidationError { get; set; }
    public List<string>? errorMessages { get; set; }
    public List<string>? successMessages { get; set; }
}


public class CheckOutInputData
{
    public Guid GymVisitId { get; set; }
}

public class CheckOutOutputData
{
    public bool isError { get; set; }
    public bool? isValidationError { get; set; }
    public List<string>? errorMessages { get; set; }
    public List<string>? successMessages { get; set; }
}