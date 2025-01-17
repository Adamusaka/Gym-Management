using backend.Entities;
using backend.Models;

namespace backend.Interfaces.Layers.BLL;

public interface IUserLogic
{
    public Task<ObtainUsersOutputData> ObtainUsers();
    // public Task<CheckInOutputData> CheckIn(CheckInInputData checkInInputData);
}

public class ObtainUsersOutputData
{
    public bool isError { get; set; }
    public List<UserJoinSubscriptionModel>? users { get; set; }
}