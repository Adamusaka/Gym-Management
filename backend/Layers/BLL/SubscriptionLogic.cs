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
public class SubscriptionLogic : ISubscriptionLogic
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserLogic> _logger;
    private readonly IDatabaseService _databaseService;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionValidation _subscriptionValidation;

    public SubscriptionLogic(IConfiguration configuration, ILogger<UserLogic> logger, IDatabaseService databaseService, ISubscriptionRepository subscriptionRepository, ISubscriptionValidation subscriptionValidation)
    {
        _configuration = configuration;
        _logger = logger;
        _databaseService = databaseService;
        _subscriptionRepository = subscriptionRepository;
        _subscriptionValidation = subscriptionValidation;
    }

    public async Task<InsertSubscriptionOutputData> InsertSubscription(InsertSubscriptionInputData insertSubscriptionInputData)
    {
        List<string> errorMessages = await _subscriptionValidation.InsertSubscription(insertSubscriptionInputData);

        if(errorMessages.Count != 0)
        {
            return new InsertSubscriptionOutputData()
            {
                isError = false,
                isValidationError = true,
                errorMessages = errorMessages
            };
        }

        SqlConnection? databaseClient = null;

        try
        {
            databaseClient = await _databaseService.Connect();

            _subscriptionRepository.databaseClient = databaseClient;

            await _subscriptionRepository.Insert(insertSubscriptionInputData.UserId, DateTime.Now, DateTime.Now.AddYears(1));

            return new InsertSubscriptionOutputData()
            {
                isError = false,
                successMessages = new List<string>() { "User has been successfully subscribed" }
            };
        } catch(Exception ex)
        {
            return new InsertSubscriptionOutputData()
            {
                isError = true
            };

            _logger.LogInformation("Error in inserting Subscription data logic");
        } finally
        {
            await databaseClient!.DisposeAsync();

            await _subscriptionRepository.databaseClient!.DisposeAsync();
        }
    }

    public async Task<DeleteSubscriptionOutputData> DeleteSubscription(DeleteSubscriptionInputData deleteSubscriptionInputData)
    {
        List<string> errorMessages = await _subscriptionValidation.DeleteSubscription(deleteSubscriptionInputData);

        if(errorMessages.Count != 0)
        {
            return new DeleteSubscriptionOutputData()
            {
                isError = false,
                isValidationError = true,
                errorMessages = errorMessages
            };
        }

        SqlConnection? databaseClient = null;

        try
        {
            databaseClient = await _databaseService.Connect();

            _subscriptionRepository.databaseClient = databaseClient;

            await _subscriptionRepository.Delete(deleteSubscriptionInputData.UserId);

            return new DeleteSubscriptionOutputData()
            {
                isError = false,
                successMessages = new List<string>() { "User has been successfully unsubscribed" }
            };
        } catch(Exception ex)
        {
            return new DeleteSubscriptionOutputData()
            {
                isError = true
            };

            _logger.LogInformation("Error in inserting Subscription data logic");
        } finally
        {
            await databaseClient!.DisposeAsync();

            await _subscriptionRepository.databaseClient!.DisposeAsync();
        }
    }
}