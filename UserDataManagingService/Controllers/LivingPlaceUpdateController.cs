using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UserDataManagingService.Controllers.Requests;
using UserDataManagingService.Controllers.Requests.PersonalInfoUpdateRequests;
using UserDataManagingService.Models;
using UserDataManagingService.Models.Repositories;
using UserDataManagingService.Services;

namespace UserDataManagingService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = (nameof(Role.DefaultUser) + "," + nameof(Role.Admin)))]
    public class LivingPlaceUpdateController : ControllerBase
    {
        //new apartment
        [HttpPost(template: ("apartmentChangeFor_{userID}"))]
        public async Task<IActionResult> ApartmentChange([FromBody] ApartmentChangingRequest request, [FromRoute] string userID)
        {
            var targetPropertie = nameof(LivingPlace.ApartmentNr);

            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _userRepository.ConvertStringToGuid(userID);
                if(userGuidId == Guid.Empty) 
                {
                    return StatusCode(400, "id converting fail");                
                }
                _logger.LogInformation("request data changing from according service");
                
                if (await _livingPlaceEditService.LivingDataChange(userGuidId, targetPropertie, request.ApartmentNr))
                {
                    _logger.LogInformation("Data updated successfully");
                    return Ok("duomenys atnaujinti");
                }

                _logger.LogError("user data changing method fail - updating wasn't succes");
                return StatusCode(400, "duomenu atnaujinti nepavyko");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //new building
        [HttpPost(template: ("buildingChangeFor_{userID}"))]
        public async Task<IActionResult> BuildingChange([FromBody] BuildingChangingRequest request, [FromRoute] string userID)
        {
            var targetPropertie = nameof(LivingPlace.BuildingNr);

            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _userRepository.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }
                _logger.LogInformation("request data changing from according service");

                if (await _livingPlaceEditService.LivingDataChange(userGuidId, targetPropertie, request.BuildingNr))
                {
                    _logger.LogInformation("Data updated successfully");
                    return Ok("duomenys atnaujinti");
                }

                _logger.LogError("user data changing method fail - updating wasn't succes");
                return StatusCode(400, "duomenu atnaujinti nepavyko");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //new city
        [HttpPost(template: ("CityChangeFor_{userID}"))]
        public async Task<IActionResult> CityChange([FromBody] CityChangingRequest request, [FromRoute] string userID)
        {
            var targetPropertie = nameof(LivingPlace.City);

            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _userRepository.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }
                _logger.LogInformation("request data changing from according service");

                if (await _livingPlaceEditService.LivingDataChange(userGuidId, targetPropertie, request.City))
                {
                    _logger.LogInformation("Data updated successfully");
                    return Ok("duomenys atnaujinti");
                }

                _logger.LogError("user data changing method fail - updating wasn't succes");
                return StatusCode(400, "duomenu atnaujinti nepavyko");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //new apartment
        [HttpPost(template: ("streetChangeFor_{userID}"))]
        public async Task<IActionResult> StreetChange([FromBody] StreetChangingRequest request, [FromRoute] string userID)
        {
            var targetPropertie = nameof(LivingPlace.Street);

            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _userRepository.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }
                _logger.LogInformation("request data changing from according service");

                if (await _livingPlaceEditService.LivingDataChange(userGuidId, targetPropertie, request.Street))
                {
                    _logger.LogInformation("Data updated successfully");
                    return Ok("duomenys atnaujinti");
                }

                _logger.LogError("user data changing method fail - updating wasn't succes");
                return StatusCode(400, "duomenu atnaujinti nepavyko");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //
        private readonly ILogger<LivingPlaceUpdateController> _logger;

        private readonly ILivingPlaceEditService _livingPlaceEditService;
        private readonly IUserRepository _userRepository;
        private readonly IJWTService _jwtService;
        public LivingPlaceUpdateController(ILivingPlaceEditService livingPlaceEditService, IUserRepository userRepository, IJWTService jwtService, ILogger<LivingPlaceUpdateController> logger)
        {
            //_personalInfoUpdateService = personalInfoUpdateService;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
            _livingPlaceEditService = livingPlaceEditService;
        }
    }
}
