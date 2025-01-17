using backend.Interfaces.Layers.BLL;

namespace backend.Interfaces.Layers.VL;

public interface IAuthenticationValidation
{
    public Task<List<string>> Signin(SigninInputData signinInputData);
    public Task<List<string>> Signup(SignupInputData signupInputData);
}