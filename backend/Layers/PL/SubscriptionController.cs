using Microsoft.AspNetCore.Mvc;
using backend.Interfaces.Layers.BLL;
using backend.Interfaces.Layers.PL;

namespace backend.Layers.PL;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ISubscriptionLogic _subscriptionLogic;

    public SubscriptionController(IConfiguration configuration, ISubscriptionLogic subscriptionLogic)
    {
        _configuration = configuration;
        _subscriptionLogic = subscriptionLogic;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionBody createSubscriptionBody)
    {
        InsertSubscriptionInputData insertSubscriptionInputData = new InsertSubscriptionInputData() {
            UserId = createSubscriptionBody.UserId
        };

        InsertSubscriptionOutputData insertSubscriptionOutputData = await _subscriptionLogic.InsertSubscription(insertSubscriptionInputData);

        if(insertSubscriptionOutputData.isError)
        {
            return StatusCode(500);
        }

        if(insertSubscriptionOutputData.isValidationError)
        {
            return StatusCode(400, new {
                errorMessages = insertSubscriptionOutputData.errorMessages
            });
        }

        return StatusCode(200, new {
            successMessages = insertSubscriptionOutputData.successMessages,
        });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteSubscription([FromBody] DeleteSubscriptionBody deleteSubscriptionBody)
    {
        DeleteSubscriptionInputData deleteSubscriptionInputData = new DeleteSubscriptionInputData() {
            UserId = deleteSubscriptionBody.UserId
        };

        DeleteSubscriptionOutputData deleteSubscriptionOutputData = await _subscriptionLogic.DeleteSubscription(deleteSubscriptionInputData);

        if(deleteSubscriptionOutputData.isError)
        {
            return StatusCode(500);
        }

        if(deleteSubscriptionOutputData.isValidationError)
        {
            return StatusCode(400, new {
                errorMessages = deleteSubscriptionOutputData.errorMessages
            });
        }

        return StatusCode(200, new {
            successMessages = deleteSubscriptionOutputData.successMessages,
        });
    }

    public class CreateSubscriptionBody
    {
        public Guid UserId { get; set; }
    }

    public class DeleteSubscriptionBody
    {
        public Guid UserId { get; set; }
    }
}