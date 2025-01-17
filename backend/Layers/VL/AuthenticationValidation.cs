using backend.Entities;
using backend.Interfaces.Layers.BLL;
using backend.Interfaces.Layers.DAL;
using backend.Interfaces.Layers.VL;
using backend.Interfaces.Services;
using Microsoft.Data.SqlClient;

namespace backend.Layers.VL;

public class AuthenticationVaildation : IAuthenticationValidation {
    private readonly ILogger<AuthenticationVaildation> _logger;
    private readonly IDatabaseService _databaseService;
    private readonly IUserRepository _userRepository;

    public AuthenticationVaildation(ILogger<AuthenticationVaildation> logger, IDatabaseService databaseService, IUserRepository userRepository)
    {
        _logger = logger;
        _databaseService = databaseService;
        _userRepository = userRepository;
    }

    public async Task<List<string>> Signin(SigninInputData signinInputData)
    {
        List<string> validationErrors = new List<string>();

        if(String.IsNullOrEmpty(signinInputData.Email))
        {
            validationErrors.Add("The e-mail field is empty");
        } else if(!signinInputData.Email.Contains("@"))
        {
            validationErrors.Add("The e-mail you provided was not a vaild e-mail address");
        } else {
            SqlConnection? databaseClient = null;

            try
            {
                databaseClient = await _databaseService.Connect();

                _userRepository.databaseClient = databaseClient;

                var usersData = await _userRepository.SelectByEmail(signinInputData.Email);

                if(usersData.Count == 0)
                {
                    validationErrors.Add("A user with that e-mail does not exist");
                }
            } catch(Exception ex)
            {
                _logger.LogInformation("Error while validating Signup route");
            } finally
            {
                await databaseClient!.DisposeAsync();

                await _userRepository.databaseClient!.DisposeAsync();
            }
        }

        if(String.IsNullOrEmpty(signinInputData.Password)) 
        {
            validationErrors.Add("The password field is empty");
        } else {
            SqlConnection? databaseClient = null;

            try
            {
                databaseClient = await _databaseService.Connect();

                _userRepository.databaseClient = databaseClient;
                
                var usersData = await _userRepository.SelectByEmail(signinInputData.Email);
                
                bool passwordIsValid;

                if(usersData.Count != 0)
                {
                    passwordIsValid = BCrypt.Net.BCrypt.Verify(signinInputData.Password, usersData[0].Password);
                } else {
                    passwordIsValid = true;
                }

                if(!passwordIsValid) {
                    validationErrors.Add("The password is incorrect");
                }
            } catch(Exception ex)
            {
                _logger.LogInformation("Error while validating Signup route");
            } finally
            {
                await databaseClient!.DisposeAsync();

                await _userRepository.databaseClient!.DisposeAsync();
            }
        }

        return validationErrors;
    }

    public async Task<List<string>> Signup(SignupInputData signupInputData)
    {
        List<string> validationErrors = new List<string>();

        if(String.IsNullOrEmpty(signupInputData.Email))
        {
            validationErrors.Add("The e-mail field is empty");
        } else if(!signupInputData.Email.Contains("@"))
        {
            validationErrors.Add("The e-mail you provided was not a vaild e-mail address");
        } else {
            SqlConnection? databaseClient = null;
            
            try
            {
                databaseClient = await _databaseService.Connect();

                _userRepository.databaseClient = databaseClient;

                List<UserEntity> usersData = await _userRepository.SelectByEmail(signupInputData.Email);

                if(usersData.Count > 0) {
                    validationErrors.Add("A user with that e-mail address already exists");
                }
            } catch(Exception ex)
            {
                _logger.LogInformation("Error while validating Signup route");
            } finally
            {
                await databaseClient!.DisposeAsync();

                await _userRepository.databaseClient!.DisposeAsync();
            }
        }

        if(String.IsNullOrEmpty(signupInputData.FirstName))
        {
            validationErrors.Add("The first name field is empty");
        } 

        if(String.IsNullOrEmpty(signupInputData.LastName))
        {
            validationErrors.Add("The last name field is empty");
        }

        if(String.IsNullOrEmpty(signupInputData.Password))
        {
            validationErrors.Add("The password field is empty");
        } else if(signupInputData.Password.Length < 8) {
            validationErrors.Add("The password length should be at least 8 characters long");
        }

        return validationErrors;
    }
}