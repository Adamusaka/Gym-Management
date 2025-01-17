using Microsoft.AspNetCore.Mvc;
using backend.Interfaces.Layers.BLL;
using backend.Interfaces.Layers.PL;
using backend.Interfaces.Layers.DAL;
using backend.Interfaces.Layers.VL;
using Microsoft.Data.SqlClient;
using backend.Interfaces.Services;
using backend.Entities;
using backend.Models;

namespace backend.Layers.BLL;

[ApiController]
[Route("api/[controller]")]
public class UserLogic : IUserLogic
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserLogic> _logger;
    private readonly IDatabaseService _databaseService;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserValidation _userValidation;

    public UserLogic(IConfiguration configuration, ILogger<UserLogic> logger, IDatabaseService databaseService, IUserRepository userRepository, ISubscriptionRepository subscriptionRepository, IUserValidation userValidation)
    {
        _configuration = configuration;
        _logger = logger;
        _databaseService = databaseService;
        _subscriptionRepository = subscriptionRepository;
        _userRepository = userRepository;
        _userValidation = userValidation;
    }

    public async Task<ObtainUsersOutputData> ObtainUsers()
    {
        SqlConnection? databaseClient = null;

        try
        {
            databaseClient = await _databaseService.Connect();

            _userRepository.databaseClient = databaseClient;
            _subscriptionRepository.databaseClient = databaseClient;

            List<UserEntity> usersData = await _userRepository.Select();

            List<UserJoinSubscriptionModel> userJoinSubscriptionModel = new List<UserJoinSubscriptionModel>();

            foreach(UserEntity userData in usersData)
            {
                List<SubscriptionEntity> subscriptionsData = await _subscriptionRepository.SelectByUserId(userData.UserId);

                userJoinSubscriptionModel.Add(new UserJoinSubscriptionModel()
                {
                    UserId = userData.UserId,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Email = userData.Email,
                    Role = userData.Role,
                    JoinDate = subscriptionsData.Count > 0 ? subscriptionsData[0].JoinDate : null,
                    EndDate = subscriptionsData.Count > 0 ? subscriptionsData[0].EndDate : null,
                    IsSubscribed = subscriptionsData.Count > 0 ? true : false
                });
            }

            return new ObtainUsersOutputData()
            {
                isError = false,
                users = (userJoinSubscriptionModel.Count == 0) ? new List<UserJoinSubscriptionModel>() : userJoinSubscriptionModel
            };
        } catch(Exception ex)
        {
            return new ObtainUsersOutputData()
            {
                isError = true
            };

            _logger.LogInformation("Error in obtaining gym visit data logic");
        } finally
        {
            await databaseClient!.DisposeAsync();

            await _userRepository.databaseClient!.DisposeAsync();
            await _subscriptionRepository.databaseClient!.DisposeAsync();
        }
    }
}