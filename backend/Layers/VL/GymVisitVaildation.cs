using Microsoft.AspNetCore.Mvc;
using backend.Interfaces.Layers.BLL;
using backend.Interfaces.Layers.PL;
using backend.Interfaces.Layers.DAL;
using backend.Interfaces.Layers.VL;
using Microsoft.Data.SqlClient;
using backend.Interfaces.Services;
using backend.Entities;

namespace backend.Layers.BLL;

[ApiController]
[Route("api/[controller]")]
public class GymVisitValidation : IGymVisitValidation
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserLogic> _logger;
    private readonly IDatabaseService _databaseService;
    private readonly IGymVisitRepository _gymVisitRepository;

    public GymVisitValidation(IConfiguration configuration, ILogger<UserLogic> logger, IDatabaseService databaseService, IGymVisitRepository gymVisitRepository)
    {
        _configuration = configuration;
        _logger = logger;
        _databaseService = databaseService;
        _gymVisitRepository = gymVisitRepository;
    }

    public async Task<List<string>> CheckIn(CheckInInputData checkInInputData)
    {
        List<string> validationErrors = new List<string>();

        if(checkInInputData.UserId == null)
        {
            validationErrors.Add("User ID field is empty");
        } else if(checkInInputData.UserId.GetType() != typeof(Guid))
        {
            validationErrors.Add("User ID field must be a valid GUID");
        } else {
            SqlConnection? databaseClient = null;
            
            try {
                databaseClient = await _databaseService.Connect();

                _gymVisitRepository.databaseClient = databaseClient;

                GymVisitEntity? gymVisitCheckedInData = (await _gymVisitRepository.SelectByUserId(checkInInputData.UserId)).SingleOrDefault(gymVisit => gymVisit.UserId == checkInInputData.UserId && gymVisit.ToDate == null);

                if(gymVisitCheckedInData != null)
                {
                    validationErrors.Add("User is already checked in");
                }
            } catch(Exception ex)
            {
                _logger.LogInformation("Error in check-in validation");
            } finally
            {
                await databaseClient!.DisposeAsync();

                await _gymVisitRepository.databaseClient!.DisposeAsync();
            }
        }

        return validationErrors;
    }

    public async Task<List<string>> CheckOut(CheckOutInputData checkOutInputData)
    {
        List<string> validationErrors = new List<string>();

        if(checkOutInputData.GymVisitId == null)
        {
            validationErrors.Add("Gym Visit ID field is empty");
        } else if(checkOutInputData.GymVisitId.GetType() != typeof(Guid))
        {
            validationErrors.Add("Gym Visit ID field must be a valid GUID");
        } else {
            SqlConnection? databaseClient = null;
            
            try {
                databaseClient = await _databaseService.Connect();

                _gymVisitRepository.databaseClient = databaseClient;

                List<GymVisitEntity>? gymVisits = await _gymVisitRepository.SelectById(checkOutInputData.GymVisitId);

                if(gymVisits[0].ToDate != null)
                {
                    validationErrors.Add("User is already checked out");
                }
            } catch(Exception ex)
            {
                _logger.LogInformation("Error in check-in validation");
            } finally
            {
                await databaseClient!.DisposeAsync();

                await _gymVisitRepository.databaseClient!.DisposeAsync();
            }
        }

        return validationErrors;
    }
}