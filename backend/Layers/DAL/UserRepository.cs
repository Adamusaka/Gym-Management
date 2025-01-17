using backend.Entities;
using backend.Interfaces.Layers.DAL;
using backend.Interfaces.Services;
using Dapper;
using Microsoft.Data.SqlClient;

namespace backend.Layers.DAL;

public class UserRepository : IUserRepository {
    public SqlConnection? databaseClient { get; set; }
    public SqlTransaction? databaseTransaction { get; set; }
    private readonly IDatabaseService _databaseService;

    public UserRepository(IConfiguration configuration, IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<UserEntity>> Select()
    {
        string SQL = $@"SELECT * FROM Users";

        List<UserEntity> UsersData = (await databaseClient!.QueryAsync<UserEntity>(SQL)).ToList<UserEntity>();

        return UsersData;
    }
    public async Task<List<UserEntity>> SelectByEmail(string Email)
    {
        string SQL = $@"SELECT * FROM Users WHERE Email = @Email";

        object parameters = new {
            Email = Email,
        };

        List<UserEntity> UsersData = (await databaseClient!.QueryAsync<UserEntity>(SQL, parameters)).ToList<UserEntity>();

        return UsersData;
    }

    public async Task<List<UserEntity>> SelectByUserId(Guid UserId)
    {
        string SQL = $@"SELECT * FROM Users WHERE UserId = @UserId";

        object parameters = new {
            UserId = UserId,
        };

        List<UserEntity> UsersData = (await databaseClient!.QueryAsync<UserEntity>(SQL, parameters)).ToList<UserEntity>();

        return UsersData;
    }

    public async Task Insert(string FirstName, string LastName, string Email, string Password, string Role)
    {
        string SQL = $@"INSERT INTO Users(FirstName, LastName, Email, Password, Role) VALUES(@FirstName, @LastName, @Email, @Password, @Role)";

        object parameters = new {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Password = Password,
            Role = Role
        };

        await databaseClient!.ExecuteAsync(SQL, parameters);
    }
}