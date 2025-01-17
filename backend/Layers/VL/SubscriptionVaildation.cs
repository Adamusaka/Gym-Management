using Microsoft.AspNetCore.Mvc;
using backend.Interfaces.Layers.BLL;
using backend.Interfaces.Layers.PL;
using backend.Interfaces.Layers.DAL;
using backend.Interfaces.Layers.VL;
using backend.Entities;
using Microsoft.Data.SqlClient;
using backend.Interfaces.Services;

namespace backend.Layers.BLL;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionValidation : ISubscriptionValidation
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserLogic> _logger;
    private readonly IDatabaseService _databaseService;
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;


    public SubscriptionValidation(IConfiguration configuration, ILogger<UserLogic> logger, IUserRepository userRepository, IDatabaseService databaseService, ISubscriptionRepository subscriptionRepository)
    {
        _configuration = configuration;
        _logger = logger;
        _databaseService = databaseService;
        _userRepository = userRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<List<string>> InsertSubscription(InsertSubscriptionInputData insertSubscriptionInputData)
    {
        List<string> validationErrors = new List<string>();

        if(insertSubscriptionInputData.UserId == null)
        {
            validationErrors.Add("User ID field is empty");
        } else if(insertSubscriptionInputData.UserId.GetType() != typeof(Guid))
        {
            validationErrors.Add("User ID field must be a valid GUID");
        } else {
            try {
                SqlConnection? databaseClient = null;

                try
                {
                    databaseClient = await _databaseService.Connect();

                    _userRepository.databaseClient = databaseClient;
                    _subscriptionRepository.databaseClient = databaseClient;

                    List<UserEntity> usersData = await _userRepository.SelectByUserId(insertSubscriptionInputData.UserId);
                    List<SubscriptionEntity> subscriptionData = await _subscriptionRepository.SelectByUserId(insertSubscriptionInputData.UserId);

                    if(usersData.Count == 0)
                    {
                        validationErrors.Add("A user with that User ID does not exist");
                    }

                    if(subscriptionData.Count > 0)
                    {
                        validationErrors.Add("The user is already subscribed");
                    }
                } catch(Exception ex)
                {
                    _logger.LogInformation("Error in inserting subscription validation");
                } finally
                {
                    await databaseClient!.DisposeAsync();

                    await _userRepository.databaseClient!.DisposeAsync();
                    await _subscriptionRepository.databaseClient!.DisposeAsync();
                }
            } catch(Exception ex)
            {
                _logger.LogInformation("Error in insert Subscription validation");
            }
        }
        return validationErrors;
    }

    public async Task<List<string>> DeleteSubscription(DeleteSubscriptionInputData deleteSubscriptionInputData)
    {
        List<string> validationErrors = new List<string>();

        if(deleteSubscriptionInputData.UserId == null)
        {
            validationErrors.Add("User ID field is empty");
        } else if(deleteSubscriptionInputData.UserId.GetType() != typeof(Guid))
        {
            validationErrors.Add("User ID field must be a valid GUID");
        } else {
            try {
                SqlConnection? databaseClient = null;

                try
                {
                    databaseClient = await _databaseService.Connect();

                    _userRepository.databaseClient = databaseClient;
                    _subscriptionRepository.databaseClient = databaseClient;

                    List<UserEntity> usersData = await _userRepository.SelectByUserId(deleteSubscriptionInputData.UserId);
                    List<SubscriptionEntity> subscriptionData = await _subscriptionRepository.SelectByUserId(deleteSubscriptionInputData.UserId);

                    if(usersData.Count == 0)
                    {
                        validationErrors.Add("A user with that User ID does not exist");
                    }

                    if(subscriptionData.Count == 0)
                    {
                        validationErrors.Add("The user is already unsubscribed");
                    }
                } catch(Exception ex)
                {
                    _logger.LogInformation("Error in inserting subscription validation");
                } finally
                {
                    await databaseClient!.DisposeAsync();

                    await _userRepository.databaseClient!.DisposeAsync();
                    await _subscriptionRepository.databaseClient!.DisposeAsync();
                }
            } catch(Exception ex)
            {
                _logger.LogInformation("Error in insert Subscription validation");
            }
        }
        return validationErrors;
    }
}