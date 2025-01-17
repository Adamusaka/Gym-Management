using backend.Entities;
using Microsoft.Data.SqlClient;

namespace backend.Interfaces.Layers.DAL;

public interface IGymVisitRepository
{
    public SqlConnection? databaseClient { get; set; }
    public SqlTransaction? databaseTransaction { get; set; }
    public Task<List<GymVisitEntity>> Select();
    public Task Insert(DateTime FromDate, Guid UserId);
    public Task UpdateToDateById(Guid GymVisitId, DateTime ToDate);
    public Task<List<GymVisitEntity>> SelectById(Guid GymVisitId);
    public Task<List<GymVisitEntity>> SelectByUserId(Guid UserId);
}