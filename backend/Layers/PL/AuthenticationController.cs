using Microsoft.AspNetCore.Mvc;
using backend.Interfaces.Layers.BLL;
using backend.Interfaces.Layers.PL;

namespace backend.Layers.PL;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAuthenticationLogic _authenticationLogic;

    public AuthenticationController(IConfiguration configuration, IAuthenticationLogic authenticationLogic)
    {
        _configuration = configuration;
        _authenticationLogic = authenticationLogic;
    }

    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> Signin([FromBody] SigninBody signinBody)
    {
        SigninInputData signinInputData = new SigninInputData() {
            Email = signinBody.Email,
            Password = signinBody.Password,
        };

        SigninOutputData signinOutputData = await _authenticationLogic.Signin(signinInputData);

        if(signinOutputData.isError)
        {
            return StatusCode(500);
        }

        if(signinOutputData.isValidationError)
        {
            return StatusCode(400, new {
                errorMessages = signinOutputData.errorMessages
            });
        }

        return StatusCode(200, new {
            successMessages = signinOutputData.successMessages,
            userId = signinOutputData.userId,
            role = signinOutputData.role,
            isSubscribed = signinOutputData.isSubscribed
        });
    }

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupBody signupBody)
    {
        SignupInputData signupInputData = new SignupInputData() {
            Email = signupBody.Email,
            FirstName = signupBody.FirstName,
            LastName = signupBody.LastName,
            Password = signupBody.Password,
            Role = "Customer"
        };

        SignupOutputData signupOutputData = await _authenticationLogic.Signup(signupInputData);

        if(signupOutputData.isError)
        {
            return StatusCode(500);
        }

        if(signupOutputData.isValidationError)
        {
            return StatusCode(400, new {
                errorMessages = signupOutputData.errorMessages
            });
        }

        return StatusCode(200, new {
            successMessages = signupOutputData.successMessages
        });
    }
}