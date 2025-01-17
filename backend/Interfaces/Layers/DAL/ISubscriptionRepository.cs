using backend.Entities;
using Microsoft.Data.SqlClient;

namespace backend.Interfaces.Layers.DAL;

public interface ISubscriptionRepository
{
    public SqlConnection? databaseClient { get; set; }
    public SqlTransaction? databaseTransaction { get; set; }
    public Task<List<SubscriptionEntity>> SelectByUserId(Guid UserId);
    public Task Insert(Guid UserId, DateTime JoinDate, DateTime EndDate);
    public Task Delete(Guid UserId);
}