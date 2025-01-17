using Microsoft.AspNetCore.Mvc;
using backend.Interfaces.Layers.BLL;
using backend.Interfaces.Layers.PL;
using backend.Interfaces.Layers.DAL;
using backend.Interfaces.Layers.VL;

namespace backend.Layers.BLL;

[ApiController]
[Route("api/[controller]")]
public class UserValidation : IUserValidation
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public UserValidation(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }
}