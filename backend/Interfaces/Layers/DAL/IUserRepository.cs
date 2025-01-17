using backend.Entities;
using Microsoft.Data.SqlClient;

namespace backend.Interfaces.Layers.DAL;

public interface IUserRepository
{
    public SqlConnection? databaseClient { get; set; }
    public SqlTransaction? databaseTransaction { get; set; }
    public Task<List<UserEntity>> Select();
    public Task<List<UserEntity>> SelectByEmail(string Email);
    public Task<List<UserEntity>> SelectByUserId(Guid UserId);
    public Task Insert(string FirstName, string LastName, string Email, string Password, string Role);
}