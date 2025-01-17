using backend.Entities;
using backend.Interfaces.Layers.DAL;
using backend.Interfaces.Services;
using Dapper;
using Microsoft.Data.SqlClient;

namespace backend.Layers.DAL;

public class SubscriptionRepository : ISubscriptionRepository {
    public SqlConnection? databaseClient { get; set; }
    public SqlTransaction? databaseTransaction { get; set; }
    private readonly IDatabaseService _databaseService;

    public SubscriptionRepository(IConfiguration configuration, IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<SubscriptionEntity>> SelectByUserId(Guid UserId)
    {
        string SQL = $@"SELECT * FROM Subscriptions WHERE UserId = @UserId";

        object parameters = new {
            UserId = UserId,
        };

        List<SubscriptionEntity> subscriptionData = (await databaseClient!.QueryAsync<SubscriptionEntity>(SQL, parameters)).ToList<SubscriptionEntity>();

        return subscriptionData;
    }

    public async Task Insert(Guid UserId, DateTime JoinDate, DateTime EndDate)
    {
        string SQL = $@"INSERT INTO Subscriptions(UserId, JoinDate, EndDate) VALUES(@UserId, @JoinDate, @EndDate)";

        object parameters = new {
            UserId = UserId,
            JoinDate = JoinDate,
            EndDate = EndDate
        };

        await databaseClient!.ExecuteAsync(SQL, parameters);
    }

    public async Task Delete(Guid UserId)
    {
        string SQL = $@"DELETE FROM Subscriptions WHERE UserId = @UserId";

        object parameters = new {
            UserId = UserId,
        };

        await databaseClient!.ExecuteAsync(SQL, parameters);
    }
}