using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
    public class PersonalInfoUpdteController : ControllerBase
    {

        //new name
        [HttpPost(template: ("NameChangeFor_{userID}"))]
        public async Task<IActionResult> NameChange([FromBody] NameChangingRequest request, [FromRoute] string userID)
        {
            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _userRepository.ConvertStringToGuid(userID);
                if(userGuidId == Guid.Empty) 
                {
                    return StatusCode(400, "id converting fail");                
                }

                var isUserAuthorisedCorrectly = await _jwtService.IsUserProvidedTokenOwner(userGuidId);
                if (!isUserAuthorisedCorrectly)
                {
                    _logger.LogWarning("requested user doesn't match the data owner (jwt provided user)");
                    return Unauthorized("thats not you!");
                }

                _logger.LogInformation("request data changing from according service");
                if (await _personalInfoUpdateService.NameChange(userGuidId, request.Name))
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

        //new email
        [HttpPost(template: ("EmailChangeFor_{userID}"))]
        public async Task<IActionResult> EmailChange([FromBody] EmailChangingRequest request, [FromRoute] string userID)
        {
            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _userRepository.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }

                var isUserAuthorisedCorrectly = await _jwtService.IsUserProvidedTokenOwner(userGuidId);
                if (!isUserAuthorisedCorrectly)
                {
                    _logger.LogWarning("requested user doesn't match the data owner (jwt provided user)");
                    return Unauthorized("thats not you!");
                }

                _logger.LogInformation("request data changing from according service");
                if (await _personalInfoUpdateService.EmailChange(userGuidId, request.Email))
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

        //new LastName
        [HttpPost(template: ("LastNameChangeFor_{userID}"))]
        public async Task<IActionResult> LastNameChange([FromBody] LastNameChangingRequest request, [FromRoute] string userID)
        {
            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _userRepository.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }

                var isUserAuthorisedCorrectly = await _jwtService.IsUserProvidedTokenOwner(userGuidId);
                if (!isUserAuthorisedCorrectly)
                {
                    _logger.LogWarning("requested user doesn't match the data owner (jwt provided user)");
                    return Unauthorized("thats not you!");
                }

                _logger.LogInformation("request data changing from according service");
                if (await _personalInfoUpdateService.LastNameChange(userGuidId, request.LastName))
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

        //new password
        [HttpPost(template: ("passwordChangeFor_{userID}"))]
        public async Task<IActionResult> PasswordChange([FromBody] PasswordChangingRequest request, [FromRoute] string userID)
        {
            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _userRepository.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }

                var isUserAuthorisedCorrectly = await _jwtService.IsUserProvidedTokenOwner(userGuidId);
                if (!isUserAuthorisedCorrectly)
                {
                    _logger.LogWarning("requested user doesn't match the data owner (jwt provided user)");
                    return Unauthorized("thats not you!");
                }

                _logger.LogInformation("request data changing from according service");
                if (await _personalInfoUpdateService.PasswordChange(userGuidId, request.OldPassword, request.NewPassword))
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

        //new Personal code
        [HttpPost(template: ("personalCodeChangeFor_{userID}"))]
        public async Task<IActionResult> PersonalCodeChange([FromBody] PersonalCodeChangingRequest request, [FromRoute] string userID)
        {
            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _userRepository.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }

                var isUserAuthorisedCorrectly = await _jwtService.IsUserProvidedTokenOwner(userGuidId);
                if (!isUserAuthorisedCorrectly)
                {
                    _logger.LogWarning("requested user doesn't match the data owner (jwt provided user)");
                    return Unauthorized("thats not you!");
                }

                _logger.LogInformation("request data changing from according service");
                if (await _personalInfoUpdateService.PersonalCodeChange(userGuidId, request.PersonalCode))
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

        //new Phone nr
        [HttpPost(template: ("phoneNrChangeFor_{userID}"))]
        public async Task<IActionResult> PhoneNrChange([FromBody] PhoneNrChangingRequest request, [FromRoute] string userID)
        {
            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _userRepository.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }

                var isUserAuthorisedCorrectly = await _jwtService.IsUserProvidedTokenOwner(userGuidId);
                if (!isUserAuthorisedCorrectly)
                {
                    _logger.LogWarning("requested user doesn't match the data owner (jwt provided user)");
                    return Unauthorized("thats not you!");
                }

                _logger.LogInformation("request data changing from according service");
                if (await _personalInfoUpdateService.PhoneNrChange(userGuidId, request.PhoneNr))
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

        [HttpPost(template: ("deactivate_{userID}"))]
        public async Task<IActionResult> DeactivateUser([FromRoute] string userID)
        {
            try
            {
                _logger.LogInformation("read user id from route and convert to guid");
                var userGuidId = _userRepository.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }

                var targetUser = await _userRepository.GetFullUserById(userGuidId);
                var currentNick = User.FindFirst(ClaimTypes.Name)?.Value;
                var isUserAuthorisedCorrectly = (targetUser.NickName == currentNick || currentNick == "admin");               
                if (!isUserAuthorisedCorrectly)
                {
                    _logger.LogWarning("requested user doesn't match the data owner (jwt provided user)");
                    return Unauthorized("thats not you!");
                }

                _logger.LogInformation("trying deactivate user - UserLoginService");
                if (await _userRepository.DeactivateUser(userGuidId))
                {
                    _logger.LogInformation("deactivated successfully");
                    return Ok("user deactyvuotas");
                }

                _logger.LogError("user wasn't deactivated");
                return StatusCode(400, "duomenu atnaujinti nepavyko");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

/*        //just another way to make extra authorization
        public async Task<bool> IsUserProvidedTokenOwner(Guid userId)
        {
            var targetUser = await _userRepository.GetFullUserById(userId);
            var currentNick = User.FindFirst(ClaimTypes.Name)?.Value;
            return targetUser?.NickName.Equals(currentNick) ?? false;
        }

        public async Task<bool> IsUserProvidedTokenOwnerOrAdmin(Guid userId)
        {
            var targetUser = await _userRepository.GetFullUserById(userId);
            var currentNick = User.FindFirst(ClaimTypes.Name)?.Value;
            return (targetUser.NickName == currentNick || targetUser?.NickName == "admin");
        }*/

        //
        private readonly ILogger<PersonalInfoUpdteController> _logger;

        private readonly IPersonalInfoUpdateService _personalInfoUpdateService;
        private readonly IUserRepository _userRepository;
        private readonly IJWTService _jwtService;
        public PersonalInfoUpdteController(IPersonalInfoUpdateService personalInfoUpdateService, IUserRepository userRepository, IJWTService jwtService, ILogger<PersonalInfoUpdteController> logger)
        {
            _personalInfoUpdateService = personalInfoUpdateService;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }
    }
}
