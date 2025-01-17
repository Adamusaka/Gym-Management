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
public class GymVisitLogic : IGymVisitLogic
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GymVisitLogic> _logger;
    private readonly IDatabaseService _databaseService;
    private readonly IGymVisitRepository _gymVisitRepository;
    private readonly IGymVisitValidation _gymVisitValidation;

    public GymVisitLogic(IConfiguration configuration, ILogger<GymVisitLogic> logger, IDatabaseService databaseService, IGymVisitRepository gymVisitRepository, IGymVisitValidation gymVisitValidation)
    {
        _configuration = configuration;
        _logger = logger;
        _databaseService = databaseService;
        _gymVisitRepository = gymVisitRepository;
        _gymVisitValidation = gymVisitValidation;
    }

    public async Task<ObtainGymVisitOutputData> ObtainGymVisit()
    {
        SqlConnection? databaseClient = null;
        
        try
        {
            databaseClient = await _databaseService.Connect();

            _gymVisitRepository.databaseClient = databaseClient;

            List<GymVisitEntity> gymVisitData = await _gymVisitRepository.Select();

            return new ObtainGymVisitOutputData()
            {
                isError = false,
                gymVisits = (gymVisitData.Count == 0) ? new List<GymVisitEntity>() : gymVisitData
            };
        } catch(Exception ex)
        {
            return new ObtainGymVisitOutputData()
            {
                isError = true
            };

            _logger.LogInformation("Error in obtaining gym visit data logic");
        } finally
        {
            await databaseClient!.DisposeAsync();

            await _gymVisitRepository.databaseClient!.DisposeAsync();
        }
    }

    public async Task<CheckInOutputData> CheckIn(CheckInInputData checkInInputData)
    {
        List<string> errorMessages = await _gymVisitValidation.CheckIn(checkInInputData);

        if(errorMessages.Count != 0)
        {
            return new CheckInOutputData()
            {
                isError = false,
                isValidationError = true,
                errorMessages = errorMessages
            };
        }

        SqlConnection? databaseClient = null;

        try
        {
            databaseClient = await _databaseService.Connect();

            _gymVisitRepository.databaseClient = databaseClient;

            await _gymVisitRepository.Insert(DateTime.Now, checkInInputData.UserId);

            return new CheckInOutputData()
            {
                isError = false,
                isValidationError = false,
                successMessages = new List<string>() { "Successfully checked-in" },
            };
        } catch(Exception ex)
        {
            return new CheckInOutputData()
            {
                isError = true,
            };

            _logger.LogInformation("Error in checkin logic");
        } finally
        {
            await databaseClient!.DisposeAsync();

            await _gymVisitRepository.databaseClient!.DisposeAsync();
        }
    }

    public async Task<CheckOutOutputData> CheckOut(CheckOutInputData checkOutInputData)
    {
        List<string> errorMessages = await _gymVisitValidation.CheckOut(checkOutInputData);

        if(errorMessages.Count != 0)
        {
            return new CheckOutOutputData()
            {
                isError = false,
                isValidationError = true,
                errorMessages = errorMessages
            };
        }

        SqlConnection? databaseClient = null;

        try
        {
            databaseClient = await _databaseService.Connect();

            _gymVisitRepository.databaseClient = databaseClient;

            await _gymVisitRepository.UpdateToDateById(checkOutInputData.GymVisitId, DateTime.Now);

            return new CheckOutOutputData()
            {
                isError = false,
                isValidationError = false,
                successMessages = new List<string>() { "Successfully checked-out" },
            };
        } catch(Exception ex)
        {

        return new CheckOutOutputData()
        {
            isError = true
        };

            _logger.LogInformation("Error in Signup logic");
        } finally
        {
            await databaseClient!.DisposeAsync();

            await _gymVisitRepository.databaseClient!.DisposeAsync();
        }
    }
}