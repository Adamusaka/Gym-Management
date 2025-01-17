using backend.Entities;
using backend.Interfaces.Layers.BLL;
using backend.Interfaces.Layers.DAL;
using backend.Interfaces.Layers.VL;
using backend.Interfaces.Services;
using backend.Layers.VL;
using Microsoft.Data.SqlClient;

namespace backend.Layers.BLL;

public class AuthenticationLogic : IAuthenticationLogic {
    private readonly ILogger<AuthenticationVaildation> _logger;
    private readonly IDatabaseService _databaseService;
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IAuthenticationValidation _authenticationValidation;

    public AuthenticationLogic(ILogger<AuthenticationVaildation> logger, IDatabaseService databaseService, IUserRepository userRepository, ISubscriptionRepository subscriptionsRepository, IAuthenticationValidation authenticationValidation)
    {
        _logger = logger;
        _databaseService = databaseService;
        _userRepository = userRepository;
        _subscriptionRepository = subscriptionsRepository;
        _authenticationValidation = authenticationValidation;
    }

    public async Task<SigninOutputData> Signin(SigninInputData signinInputData)
    {
        List<string> errorMessages = await _authenticationValidation.Signin(signinInputData);

        if(errorMessages.Count != 0)
        {
            return new SigninOutputData()
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

            _userRepository.databaseClient = databaseClient;
            _subscriptionRepository.databaseClient = databaseClient;

            List<UserEntity> users = await _userRepository.SelectByEmail(signinInputData.Email);
            List<SubscriptionEntity> SubscriptionsData = await _subscriptionRepository.SelectByUserId(users[0].UserId);

            return new SigninOutputData()
            {
                isError = false,
                isValidationError = false,
                successMessages = new List<string>() { "Successfully signed in" },
                userId = users[0].UserId,
                role = users[0].Role,
                isSubscribed = SubscriptionsData.Count > 0 ? true : false
            };
        } catch(Exception ex)
        {

        return new SigninOutputData()
        {
            isError = true,
            isValidationError = false,
        };

            _logger.LogInformation("Error in Signup logic");
        } finally
        {
            await databaseClient!.DisposeAsync();

            await _userRepository.databaseClient!.DisposeAsync();
            await _subscriptionRepository.databaseClient!.DisposeAsync();
        }
    }

    public async Task<SignupOutputData> Signup(SignupInputData signupInputData)
    {
        List<string> errorMessages = await _authenticationValidation.Signup(signupInputData);

        if(errorMessages.Count != 0)
        {
            return new SignupOutputData()
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

            _userRepository.databaseClient = databaseClient;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(signupInputData.Password, 12);

            await _userRepository.Insert(signupInputData.FirstName, signupInputData.LastName, signupInputData.Email, hashedPassword, signupInputData.Role);

            return new SignupOutputData()
            {
                isError = false,
                isValidationError = false,
                successMessages = new List<string>() { $"Successfully signed up" }
            };
        } catch(Exception ex)
        {
            return new SignupOutputData()
            {
                isError = true,
                isValidationError = false,
            };

            _logger.LogInformation("Error in Signup logic");
        } finally
        {
            await databaseClient!.DisposeAsync();

            await _userRepository.databaseClient!.DisposeAsync();
        }
    }
}