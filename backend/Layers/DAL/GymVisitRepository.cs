using backend.Entities;
using backend.Interfaces.Layers.DAL;
using backend.Interfaces.Services;
using Dapper;
using Microsoft.Data.SqlClient;

namespace backend.Layers.DAL;

public class GymVisitRepository : IGymVisitRepository {
    public SqlConnection? databaseClient { get; set; }
    public SqlTransaction? databaseTransaction { get; set; }
    private readonly IDatabaseService _databaseService;

    public GymVisitRepository(IConfiguration configuration, IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<GymVisitEntity>> Select()
    {
        string SQL = $@"SELECT * FROM GymVisits";

        List<GymVisitEntity> GymVisitData = (await databaseClient!.QueryAsync<GymVisitEntity>(SQL)).ToList<GymVisitEntity>();

        return GymVisitData;
    }

    public async Task Insert(DateTime FromDate, Guid UserId)
    {
        string SQL = $@"INSERT INTO GymVisits(FromDate, UserId) VALUES(@FromDate, @UserId)";

        object parameters = new {
            FromDate = FromDate,
            UserId = UserId,
        };

        await databaseClient!.ExecuteAsync(SQL, parameters);
    }

    public async Task UpdateToDateById(Guid GymVisitId, DateTime ToDate)
    {
        string SQL = $@"UPDATE GymVisits SET ToDate = @ToDate WHERE GymVisitId = @GymVisitId";

        object parameters = new {
            GymVisitId = GymVisitId,
            ToDate = ToDate,
        };

        await databaseClient!.ExecuteAsync(SQL, parameters);
    }

    public async Task<List<GymVisitEntity>> SelectById(Guid GymVisitId)
    {
        string SQL = $@"SELECT * GymVisits WHERE GymVisitId = @GymVisitId";

        object parameters = new {
            UserId = GymVisitId
        };


        List<GymVisitEntity> GymVisitData = (await databaseClient!.QueryAsync<GymVisitEntity>(SQL)).ToList<GymVisitEntity>();

        return GymVisitData;
    }

    public async Task<List<GymVisitEntity>> SelectByUserId(Guid UserId)
    {
        string SQL = $@"SELECT * GymVisits WHERE UserId = @UserId";

        object parameters = new {
            UserId = UserId
        };


        List<GymVisitEntity> GymVisitData = (await databaseClient!.QueryAsync<GymVisitEntity>(SQL)).ToList<GymVisitEntity>();

        return GymVisitData;
    }
}