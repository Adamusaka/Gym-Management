using backend.Interfaces.Layers.BLL;

namespace backend.Interfaces.Layers.VL;

public interface ISubscriptionValidation
{
    public Task<List<string>> InsertSubscription(InsertSubscriptionInputData insertSubscriptionInputData);
    public Task<List<string>> DeleteSubscription(DeleteSubscriptionInputData deleteSubscriptionInputData);
}