using backend.Entities;
using backend.Models;

namespace backend.Interfaces.Layers.BLL;

public interface ISubscriptionLogic
{
    public Task<InsertSubscriptionOutputData> InsertSubscription(InsertSubscriptionInputData insertSubscriptionInputData);
    public Task<DeleteSubscriptionOutputData> DeleteSubscription(DeleteSubscriptionInputData deleteSubscriptionInputData);
}

public class InsertSubscriptionInputData
{
    public Guid UserId { get; set; }
}

public class InsertSubscriptionOutputData
{
    public bool isError { get; set; }
    public bool isValidationError { get; set; }
    public List<string>? errorMessages { get; set; }
    public List<string>? successMessages { get; set; }
}

public class DeleteSubscriptionInputData
{
    public Guid UserId { get; set; }
}

public class DeleteSubscriptionOutputData
{
    public bool isError { get; set; }
    public bool isValidationError { get; set; }
    public List<string>? errorMessages { get; set; }
    public List<string>? successMessages { get; set; }
}