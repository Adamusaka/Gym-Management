using Microsoft.AspNetCore.Mvc;
using backend.Interfaces.Layers.BLL;
using backend.Interfaces.Layers.PL;

namespace backend.Layers.PL;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserLogic _userLogic;

    public UserController(IConfiguration configuration, IUserLogic userLogic)
    {
        _configuration = configuration;
        _userLogic = userLogic;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        ObtainUsersOutputData obtainUsersOutputData = await _userLogic.ObtainUsers();

        if(obtainUsersOutputData.isError)
        {
            return StatusCode(500);
        }

        return StatusCode(200, new {
            users = obtainUsersOutputData.users
        });
    }
}