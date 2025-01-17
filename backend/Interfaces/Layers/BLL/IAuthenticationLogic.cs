namespace backend.Interfaces.Layers.BLL;

public interface IAuthenticationLogic
{
    public Task<SigninOutputData> Signin(SigninInputData signinInputData);
    public Task<SignupOutputData> Signup(SignupInputData signupInputData);
}

public class SigninInputData
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class SigninOutputData
{
    public bool isError { get; set; }
    public bool isValidationError { get; set; }
    public List<string>? errorMessages { get; set; }
    public List<string>? successMessages { get; set; }
    public Guid? userId { get; set; }
    public string? role { get; set; }
    public bool? isSubscribed { get; set; }
}

public class SignupInputData
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "Customer";
}

public class SignupOutputData
{
    public bool isError { get; set; }
    public bool isValidationError { get; set; }
    public List<string>? errorMessages { get; set; }
    public List<string>? successMessages { get; set; }
}