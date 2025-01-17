using Microsoft.Data.SqlClient;

namespace backend.Interfaces.Services;

public interface IDatabaseService
{
    public Task<SqlConnection> Connect();
}