using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using UserDataManagingService.Controllers.Requests;
using UserDataManagingService.Models;
using UserDataManagingService.Models.Repositories;
using UserDataManagingService.Services;

namespace UserDataManagingService.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // autorization test
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = (nameof(Role.DefaultUser) + "," + nameof(Role.Admin)))]
        [HttpGet(template: "TouchMeIfYouAreАuthenticated !")]
        public ActionResult<string> BeNiceForAutetified()
        {
            string heyBro = "hey bro!";
            return heyBro;
        }

        [HttpPost(template: ("SignUpNewUser"))]
        public async Task<IActionResult> SignUpNewAcc([FromBody] SignUpNewUserRequest request)
        {
            try
            {
                var newAcc = await _loginService.SignupNewUser(request.Username, request.UserLastName, request.NickName, request.Password, request.PersonalCode, request.PhoneNr, request.Email);
                if (newAcc.Item1 == true) //requested user nickname exist already 
                {
                    _logger.LogInformation("requested Nickname exist already");
                    return StatusCode(400, "requested NickName is not availible");
                }
                else
                {
                    _logger.LogInformation("new user personal info recorded");
                    return Ok(newAcc.Item2);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("FinalizingUserCreationFor_{userID}")]
        public async Task<IActionResult> ConfirmUserCreationaByUserIdRouteProvided([FromRoute] string userID)
        {
            try
            {
                _logger.LogInformation("read user id from route and convert to guid, check L.Place and Avatar for null");
                var userGuidId = _loginService.ConvertStringToGuid(userID);
                if (userGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }

                if (await _livingPlaceRepository.GetLivingPlaceDataByUserID(userGuidId) == null)
                {
                    _logger.LogWarning("Living place of User is Null. This method can't be requested in such way");
                    return StatusCode(400, "nera user living place");
                }

                if (await _avatarRepository.GetAvatarByUserID(userGuidId) == null)
                {
                    _logger.LogWarning("Avatar of User is Null. This method can't be requested in such way");
                    return StatusCode(400, "nera user avatar");
                }

                _logger.LogInformation("checking user data and activation status");
                var isUserDataOk = await _loginService.CompleteUserCreating(userGuidId);
                var userChekingOkAndProfileIsActivated = isUserDataOk.Item1;
                if (userChekingOkAndProfileIsActivated == true)
                {
                    return Ok(isUserDataOk.Item2); //action comments
                }
                else
                {
                    _logger.LogWarning("user data are not correct ir status isnt active");
                    return BadRequest(isUserDataOk.Item2);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost(template: "UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _loginService.UserLogin(request.NickName, request.Password);
                if (!response.IsUserExist)
                {
                    return NotFound("user nerastas arba blogas slaptazodis");
                    //return Unauthorized();
                }
                var userId = _userRepository.GetUserIdByNickname(request.NickName); 
                return Ok(new { Token = _jwtService.GetJwtToken(request.NickName, (Role)response.Role), UserId = userId.Result });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = (nameof(Role.DefaultUser) + "," + nameof(Role.Admin)))]
        [HttpGet(template: "allDataFor_{userID}")]
        public async Task<IActionResult> ShowAllUserData([FromRoute] string userID)
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

                var userDTO = await _loginService.CreateUserDTO(targetUser);
                return Ok(userDTO);

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = nameof(Role.Admin))]        
        //delete user for admin only
        [HttpPost(template: ("delete_{adminID}"))]
        public async Task<IActionResult> DeleteUser([FromRoute] string adminID, [FromBody] UserToRemoveRequest request)
        {
            try
            {
                _logger.LogInformation("read admin and user id from route and convert to guid");
                var adminGuidId = _userRepository.ConvertStringToGuid(adminID);
                var userGuidId = _userRepository.ConvertStringToGuid(request.UserId);
                if (userGuidId == Guid.Empty || adminGuidId == Guid.Empty)
                {
                    return StatusCode(400, "id converting fail");
                }
                var checkAdminRole = await _userRepository.GetUserRoleById(adminGuidId);
                if (checkAdminRole != "Admin")
                {
                    _logger.LogWarning("permission denied");
                    return StatusCode(400, "neturite teises trinti useriu");
                }

                var userById = await _userRepository.GetFullUserById(userGuidId);
                if(userById == null)
                {
                    _logger.LogInformation("user by uploaded id wasn't founded");
                    return StatusCode(400, "nerastas user su tokiu id");
                }

                _logger.LogInformation("trying delete user - UserLoginService");
                if (await _userRepository.DeleteUserAsync(userGuidId))
                {
                    _logger.LogInformation("User removed successfully");
                    return Ok("user pasalintas");
                }

                _logger.LogError("user wasn't deleted");
                return StatusCode(400, "duomenu atnaujinti nepavyko");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        
        //
        private readonly ILogger<AuthenticationController> _logger;

        private readonly IUserRepository _userRepository;
        private readonly IUserLoginService _loginService;
        private readonly IJWTService _jwtService;
        private readonly ILivingPlaceRepository _livingPlaceRepository;
        private readonly IAvatarRepository _avatarRepository;
        public AuthenticationController(ILogger<AuthenticationController> logger, IUserRepository userRepository, IUserLoginService loginService, IJWTService jwtService, ILivingPlaceRepository livingPlaceRepository, IAvatarRepository avatarRepository)
        {
            _userRepository = userRepository;
            _loginService = loginService;
            _jwtService = jwtService;
            _logger = logger;
            _livingPlaceRepository = livingPlaceRepository;
            _avatarRepository = avatarRepository;

        }
       
    }
}
