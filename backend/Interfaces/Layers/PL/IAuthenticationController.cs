namespace backend.Interfaces.Layers.PL;

public class SigninBody {
    public string Email { get; set; }
    public string Password { get; set; }
}

public class SignupBody {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}