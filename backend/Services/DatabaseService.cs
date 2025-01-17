using backend.Interfaces.Services;
using Microsoft.Data.SqlClient;

namespace backend.Services;

public class DatabaseService : IDatabaseService
{
    private readonly ILogger<DatabaseService> _logger;
    private readonly IConfiguration _configuration;

    public DatabaseService (ILogger<DatabaseService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<SqlConnection> Connect()
    {
        try
        {
            SqlConnection connetcion = new SqlConnection(_configuration.GetConnectionString("dockerLocal"));

            await connetcion.OpenAsync();

            return connetcion;
        } catch (Exception ex)
        {
            throw new Exception("Couldn't connect to the database, there is something wrong with the configuration");
        }
    }
}