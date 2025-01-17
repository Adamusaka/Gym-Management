using Microsoft.AspNetCore.Mvc;
using backend.Interfaces.Layers.BLL;
using backend.Interfaces.Layers.PL;

namespace backend.Layers.PL;

[ApiController]
[Route("api/[controller]")]
public class GymVisitController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IGymVisitLogic _gymVisitLogic;

    public GymVisitController(IConfiguration configuration, IGymVisitLogic gymVisitLogic)
    {
        _configuration = configuration;
        _gymVisitLogic = gymVisitLogic;
    }

    [HttpGet]
    public async Task<IActionResult> GetGymVisit()
    {
        ObtainGymVisitOutputData obtainGymVisitOutputData = await _gymVisitLogic.ObtainGymVisit();

        if(obtainGymVisitOutputData.isError)
        {
            return StatusCode(500);
        }

        return StatusCode(200, new {
            gymVisits = obtainGymVisitOutputData.gymVisits
        });
    }

    [HttpPost]
    [Route("checkin")]
    public async Task<IActionResult> CheckIn([FromBody] CheckInBody checkInBody)
    {
        CheckInInputData checkInInputData = new CheckInInputData() {
            UserId = checkInBody.UserId
        };

        CheckInOutputData checkInOutputData = await _gymVisitLogic.CheckIn(checkInInputData);

        if(checkInOutputData.isError)
        {
            return StatusCode(500);
        }

        if((bool)checkInOutputData.isValidationError)
        {
            return StatusCode(400, new {
                errorMessages = checkInOutputData.errorMessages
            });
        }

        return StatusCode(200, new {
            successMessages = checkInOutputData.successMessages,
        });
    }

    [HttpPatch]
    [Route("checkout")]
    public async Task<IActionResult> CheckOut([FromBody] CheckOutBody checkOutBody)
    {
        CheckOutInputData checkOutInputData = new CheckOutInputData() {
            GymVisitId = checkOutBody.GymVisitId,
        };

        CheckOutOutputData checkOutOutputData = await _gymVisitLogic.CheckOut(checkOutInputData);

        if(checkOutOutputData.isError)
        {
            return StatusCode(500);
        }

        if((bool)checkOutOutputData.isValidationError)
        {
            return StatusCode(400, new {
                errorMessages = checkOutOutputData.errorMessages
            });
        }

        return StatusCode(200, new {
            successMessages = checkOutOutputData.successMessages,
        });
    }
}